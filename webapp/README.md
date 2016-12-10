# Wish Tree - WebApp

## Setup

### Aliyuncli

Aliyuncli only works on python2.

```sh
$ pip2 install aliyuncli
$ pip2 install aliyun-python-sdk-ecs
```

### Webapp

The server will user python3 inside a virtualenv.

```sh
$ # create config.py and set config parameters
$ virtualenv -p python3.5 env
$ source env/bin/activate
(env)$ pip install -r requirements.txt
(env)$ python manage.py init_db
```

## Running

```sh
$ source env/bin/activate
(env)$ python app.py
```
