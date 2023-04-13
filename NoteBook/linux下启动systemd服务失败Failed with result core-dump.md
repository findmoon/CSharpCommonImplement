
服务启动失败：

```sh
# sudo systemctl start my-test-worker
Job for my-test-worker.service failed because a fatal signal was delivered causing the control process to dump core.
See "systemctl status my-test-worker.service" and "journalctl -xe" for details.
```

`journalctl -xe` 查看错误信息：

```sh
# journalctl -xe
-- Subject: Unit system-systemd\x2dcoredump.slice has finished start-up
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
--
-- Unit system-systemd\x2dcoredump.slice has finished starting up.
--
-- The start-up result is done.
Apr 13 17:34:46 localhost.localdomain systemd[1]: Started Process Core Dump (PID 6078/UID 0).
-- Subject: Unit systemd-coredump@0-6078-0.service has finished start-up
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
--
-- Unit systemd-coredump@0-6078-0.service has finished starting up.
--
-- The start-up result is done.
Apr 13 17:34:46 localhost.localdomain systemd-coredump[6079]: Process 6077 (WatchFolderFile) of user 0 dumped core.

-- Unit dnf-makecache.service has finished starting up.
--
-- The start-up result is done.
Apr 13 17:33:14 localhost.localdomain pure-ftpd[6047]: (?@192.168.104.252) [INFO] New connection from 192.168.104.252
Apr 13 17:33:16 localhost.localdomain pure-ftpd[6047]: (?@192.168.104.252) [INFO] ftp is now logged in
Apr 13 17:34:46 localhost.localdomain sudo[6074]:     root : TTY=pts/0 ; PWD=/usr/sbin/MyTestLinuxWorker ; USER=root ; COMMAND=/bin/sy>
Apr 13 17:34:46 localhost.localdomain sudo[6074]: pam_systemd(sudo:session): Cannot create session: Already running in a session or us>
Apr 13 17:34:46 localhost.localdomain sudo[6074]: pam_unix(sudo:session): session opened for user root by root(uid=0)
Apr 13 17:34:46 localhost.localdomain systemd[1]: Starting my-test-worker...
-- Subject: Unit my-test-worker.service has begun start-up
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
--
-- Unit my-test-worker.service has begun starting up.
Apr 13 17:34:46 localhost.localdomain kernel: WatchFolderFile[6077]: segfault at 8 ip 00007fd5c48702b5 sp 00007fffdbd0c500 error 4 in >
Apr 13 17:34:46 localhost.localdomain kernel: Code: 81 39 52 e5 74 64 0f 84 99 09 00 00 48 85 c0 75 e4 48 83 3d c4 eb 20 00 00 0f 85 a>
Apr 13 17:34:46 localhost.localdomain systemd[1]: Created slice system-systemd\x2dcoredump.slice.
-- Subject: Unit system-systemd\x2dcoredump.slice has finished start-up
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
--
-- Unit system-systemd\x2dcoredump.slice has finished starting up.
--
-- The start-up result is done.
Apr 13 17:34:46 localhost.localdomain systemd[1]: Started Process Core Dump (PID 6078/UID 0).
-- Subject: Unit systemd-coredump@0-6078-0.service has finished start-up
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
--
-- Unit systemd-coredump@0-6078-0.service has finished starting up.
--
-- The start-up result is done.
Apr 13 17:34:46 localhost.localdomain systemd-coredump[6079]: Process 6077 (WatchFolderFile) of user 0 dumped core.

                                                              Stack trace of thread 6077:
                                                              #0  0x00007fd5c48702b5 audit_list_add_dynamic_tag (/usr/lib64/ld-2.28.so)
                                                              #1  0x00007fd5c486cfbe _dl_sysdep_start (/usr/lib64/ld-2.28.so)
                                                              #2  0x00007fd5c486e8ab _dl_start_final (/usr/lib64/ld-2.28.so)
                                                              #3  0x00007fd5c486d958 _start (/usr/lib64/ld-2.28.so)
-- Subject: Process 6077 (WatchFolderFile) dumped core
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
-- Documentation: man:core(5)
--
-- Process 6077 (WatchFolderFile) crashed and dumped core.
--
-- This usually indicates a programming error in the crashing program and
-- should be reported to its vendor as a bug.
Apr 13 17:34:46 localhost.localdomain systemd[1]: my-test-worker.service: Main process exited, code=dumped, status=11/SEGV
Apr 13 17:34:46 localhost.localdomain systemd[1]: my-test-worker.service: Failed with result 'core-dump'.
-- Subject: Unit failed
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
--
-- The unit my-test-worker.service has entered the 'failed' state with result 'core-dump'.
Apr 13 17:34:46 localhost.localdomain systemd[1]: Failed to start my-test-worker.
-- Subject: Unit my-test-worker.service has failed
-- Defined-By: systemd
-- Support: https://access.redhat.com/support
--
-- Unit my-test-worker.service has failed.
--
-- The result is failed.
```

直接启动时报错 - 段错误：

```sh
# ./WatchFolderFiles
Segmentation fault (core dumped)
```
