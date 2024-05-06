import os


class AppConfig:
    HOST = os.environ.get('HOST', '0.0.0.0')
    PORT = os.environ.get('PORT', 13500)
    LOG_LEVEL = os.environ.get('LOG_LEVEL', 'info')
    SERVICE_ACCOUNT = os.environ.get('SERVICE_ACCOUNT')
    SERVICE_ACCOUNT_KEY = os.environ.get('SERVICE_ACCOUNT_KEY', 'credentials.json')