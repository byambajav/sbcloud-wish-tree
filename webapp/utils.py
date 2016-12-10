# -*- coding: utf-8 -*-


import config

import requests

from models import User, db


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
