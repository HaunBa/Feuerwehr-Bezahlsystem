import sys
import os
from subprocess import check_output as run
import requests

import config
import schedule
import time


class GitUpdater:
    def start(self):
        schedule.every(60).seconds.do(self._check_for_update)
        while True:
            schedule.run_pending()
            time.sleep(1)

    def _check_for_update(self):
        local_version = run('git rev-parse @')
        run('git fetch origin')
        remote_version = run('git rev-parse origin')
        if config.DEBUG:
            print("Local version:" + str(local_version))
            print("Remote version:" + str(remote_version))
            return
        if local_version != remote_version:
            self._on_new_version()

    @staticmethod
    def _on_new_version():
        try:
            requests.post(f'http://localhost:{config.PORT}/shutdown', timeout=1)
        except:
            pass
        run('git reset --hard HEAD')
        run('git pull')
        sys.stdout.flush()
        os.execl(sys.executable, '"{}"'.format(sys.executable), *sys.argv)