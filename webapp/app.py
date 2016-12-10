# -*- coding: utf-8 -*-

import json

from flask import Flask, request

from message import SendMessage
from models import db, Device
from utils import get_user, find_in_list

__author__ = 'no_idea'

APP_ROOT = '/'
API_ROOT = '/api/'

FB_WEBHOOK = 'fb'
WISH_MESSAGE_HOOK = 'wishmessage'

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///wish-tree.sqlite3'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
db.init_app(app)


GREETINGS = ['hi', 'hii', 'hiii', 'hey', 'heey', 'yo', 'yoo', 'hello', 'howdy',
             'good']


@app.route(APP_ROOT, methods=["GET"])
def index():
    return 'Welcome!'


@app.route(API_ROOT + FB_WEBHOOK, methods=["GET"])
def fb_webhook():
    verification_code = 'WISH_TREE_VERIFICIATION_CODE'
    verify_token = request.args.get('hub.verify_token')
    if verification_code == verify_token:
        return request.args.get('hub.challenge')


@app.route(API_ROOT + WISH_MESSAGE_HOOK, methods=["GET"])
def wishmessage_hook():
    message = request.args.get('message')
    serial = request.args.get('serial')
    print("{} says message:{} serial:{}".format(request.remote_addr, message,
                                                serial))
    d = Device.query.filter_by(serial=serial).first()
    if d is None:
        return "invalid serial"
    else:
        return d.message


def register_device(user, serial, message):
    d = Device.query.filter_by(serial=serial).first()
    sm = SendMessage(user.sender_id)
    if d is None:
        d = Device(serial, user.id, message)
        db.session.add(d)
        db.session.commit()
        sm.build_text_message('Registered {} with message "{}".'.format(
            d.serial, message)).send_message()
    else:
        sm.build_text_message('{} was already registered.'.format(
            d.serial)).send_message()


def unregister_device(user, serial):
    pass


@app.route(API_ROOT + FB_WEBHOOK, methods=['POST'])
def fb_receive_message():
    message_entries = json.loads(request.data.decode('utf8'))['entry']
    for entry in message_entries:
        messagings = entry['messaging']
        for message in messagings:
            sender_id = message['sender']['id']
            user = get_user(sender_id)
            sm = SendMessage(sender_id)
            if message.get('message'):
                msg = message['message']
                if 'text' in msg:
                    text = msg['text']
                    print("{} says {}".format(user.sender_id, text))
                    resp = ':)'
                    if text.lower().rstrip().lstrip() in GREETINGS and len(
                            user.first_name) > 0:
                        resp = 'Hey, ' + user.first_name

                    # if text.lower().lstrip().startswith('register '):
                    words = text.lower().split()
                    r_idx = find_in_list(words, 'register')
                    m_idx = find_in_list(words, 'message')
                    if r_idx >= 0 and r_idx < m_idx and m_idx < len(
                            words) - 1:
                        words_ = text.split()
                        register_device(
                            user,
                            words_[r_idx + 1],
                            ' '.join(words_[m_idx + 1:])
                        )
                        return "register"

                    r_idx = find_in_list(words, 'unregister')
                    if r_idx >= 0 and r_idx < len(words) - 1:
                        unregister_device(user, words[r_idx + 1])
                        return "unregister"

                    # echo
                    sm.build_text_message(resp).send_message()
    return "Hi"


if __name__ == '__main__':
    # context = ('ssl/fullchain.pem', 'ssl/privkey.pem')
    app.run(host='0.0.0.0', debug=True)
