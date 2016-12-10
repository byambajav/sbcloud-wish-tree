# -*- coding: utf-8 -*-


from flask_script import Manager

from app import app, db

manager = Manager(app)


@manager.command
def init_db():
    db.drop_all(bind=None)
    db.create_all()


if __name__ == "__main__":
    manager.run()
