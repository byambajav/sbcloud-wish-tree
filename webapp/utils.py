# -*- coding: utf-8 -*-


import config
import os.path
from random import choice
from string import ascii_lowercase

import requests

from gtts import gTTS
from models import User, db
from pydub import AudioSegment


def get_user(sender_id):
    user = User.query.filter_by(sender_id=sender_id).first()

    if user is None:
        print("Not registered")
        post_message_url = (
            'https://graph.facebook.com/v2.6/{}?access_token=' +
            config.FB_TOKEN
        )
        url = post_message_url.format(sender_id).encode('utf-8')
        json = requests.get(url).json()
        if json is not None and 'first_name' in json:
            first_name = json['first_name']
            last_name = json['last_name']
            img_url = json['profile_pic']
            user = User(sender_id, first_name, last_name, img_url)
        else:
            user = User(sender_id, '', '', '')

        print('Created user:', str(user))
        db.session.add(user)
        db.session.commit()
    elif user.first_name == '':
        print("Registered but unauthorized user:", str(user))
        post_message_url = (
            'https://graph.facebook.com/v2.6/{}?access_token=' +
            config.FB_TOKEN
        )
        url = post_message_url.format(sender_id).encode('utf-8')
        json = requests.get(url).json()
        if json is not None and 'first_name' in json:
            user.first_name = json['first_name']
            user.last_name = json['last_name']
            user.img_url = json['profile_pic']
            print('Updated user:', str(user))
            db.session.commit()
        else:
            print('Could not update user:', str(user))
    else:
        print("Registered user:", str(user))

    return user


def find_in_list(l, e):
    try:
        index_element = l.index(e)
        return index_element
    except ValueError:
        return -1


def update_message_mp3_path(device, force=False):
    if not force and device.message_mp3_path != '' and os.path.exists(
            device.message_mp3_path) and device.message_mp3_path.endswith(
                'ogg'):
        return
    tts = gTTS(text=device.message, lang='en')
    path = 'static/message_mp3s/{}_{}.mp3'.format(device.id, ''.join(
        choice(ascii_lowercase) for i in range(12)))
    tts.save(path)

    # then convert to OGG
    clip = AudioSegment.from_mp3(path)
    ogg_path = path[:-3] + 'ogg'
    clip.export(ogg_path, format='ogg')

    device.message_mp3_path = ogg_path
    db.session.commit()
