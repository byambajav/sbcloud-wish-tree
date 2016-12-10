#!/usr/bin/python
# -*- coding: utf-8 -*-

from enum import Enum
from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()


class UserStatus(Enum):
    START = 'start'


class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    sender_id = db.Column(db.String(128), unique=True)
    first_name = db.Column(db.String(256))
    last_name = db.Column(db.String(256))
    img_url = db.Column(db.String(1024))
    status = db.Column(db.String(64), default=UserStatus.START.value)

    def __init__(self, sender_id, first_name, last_name, img_url):
        self.sender_id = sender_id
        self.first_name = first_name
        self.last_name = last_name
        self.img_url = img_url

    def __repr__(self):
        return 'User: sender_id={}, first_name={}, last_name={})'.format(
            self.sender_id, self.first_name, self.last_name)


class Device(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    serial = db.Column(db.String(1024))
    user_id = db.Column(db.ForeignKey(u'user.id'), nullable=False,
                        index=True)

    def __init__(self, serial, user_id):
        self.serial = serial
        self.user_id = user_id

    def __repr__(self):
        return 'User: serial={}, user_id={}'.format(self.serial, self.user_id)
