9 Advanced MySQL Security Tips

> [9 Advanced MySQL Security Tips](https://www.makeuseof.com/mysql-advanced-security-tips/)

Secure your MySQL database server by following these easy tips.

  

Readers like you help support MUO. When you make a purchase using links on our site, we may earn an affiliate commission. [Read More.](https://www.makeuseof.com/page/terms-of-use/)

MySQL is one of the most popular relational database management systems that is a jackpot for attackers trying to sneak into the databases. A newly-installed MySQL database server can have many vulnerabilities and loopholes. Since data security is of great importance, it's mandatory to understand every aspect of MySQL security.

This article focuses on the auditing and security of your MySQL database and provides nine tips to harden its security.

window.addEventListener('DOMContentLoaded', () => { $vvvInit('adsninja-ad-unit-characterCountRepeatable1-5f42160cbd920c', 'MUO\_Video\_Desktop', \['https://video.adsninja.ca/valnetinc/MakeUseOf/63eede1942350-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/63f7d368607ff-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/63f7d3a0273d6-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/63f7d4145084e-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/63eed8ec22b2e-projectRssVideoFile.mp4'\]) })

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-connectedBelowAd-5f4216389a0bf8'); });

## 1\. Avoid Unnecessary Privilege Grants

[MySQL](https://www.makeuseof.com/mysql-list-databases/) allows several privilege statements which when assigned unnecessarily to an underprivileged user can lead to reading/writing files and subverting other user privileges. Some of the most potentially risky privilege statements are FILE, GRANT, SUPER, ALTER, DELETE, SHUTDOWN, and so on. You can read more about these from MySQL official documentation. Hence, do not assign superuser privileges like FILE, GRANT, SUPER, and PROCESS to non-administrative accounts. You can revoke these unnecessary global, database, and table-level permissions as follows:

 `REVOKE ALL ON *.* FROM 'user_name'@'host_name'; #Global privileges` 

 `REVOKE CREATE,DROP ON database_name.* FROM 'user_name'@'host_name'; #Database privileges` 

 `REVOKE INSERT, UPDATE,DELETE ON database_name.table_name FROM 'user_name'@'host_name'; #Table privileges  
  
flush privileges;` 

## 2\. Restrict Remote Logins

Remote access eases the job of database administrators, but it risks the server to potential vulnerabilities and exploits. You can disable remote access for all types of MySQl user accounts by adding a skip-networking variable to the main configuration file and restarting the service.

 `[mysqld]  
skip-networking  
sudo service mysql restart` 

Similarly, you must disable root account access, if not all to restrict root account remote logins. This precaution prevents bruteforcing the MySQL root account.

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT2'); });

 `mysql> delete from mysql.user where user='root' and host not in ('localhost', '127.0.0.1', '::1'); mysql> flush privileges; ` 

## 3\. Disable Functions (load\_file, outfile, dumpfile)

Another precaution to secure MySQL against local file injection is to disable functions accessible only via the FILE grant privilege. The FILE is an option that enables low-privilege users with global command options to read or write files on the server.

- load\_file

The load\_file function loads the file content from the server as a string. For instance, the following command will load all content from the [**/etc/passwd**](https://www.makeuseof.com/etc-passwd-file-linux/) file as follows:

 `select load_file('/etc/passwd')` 

- outfile

Similarly, the outfile function writes content to the local server files. Attackers can use this function to write a payload to the file in the server, as follows:

 `select 'Local File SQL Injection' into outfile '/tmp/file.txt';  
cat /tmp/file.txt` 

Output:

 `Local File SQL Injection` 

- dumpfile

This function uses the select cause to write to the file without returning output to the screen.

 `cat /tmp/file.txt  
select 'Hello world!' into dumpfile '/tmp/world';   
` 

Output:

 `Query OK, 1 row affected (0.001 sec)` 

You can disable these functions by revoking the FILE privilege as follows:

 `revoke FILE on *.* from 'user_name'@'localhost';` 

Related: [A Beginner's Guide to Metasploit in Kali Linux (With Practical Examples)](https://www.makeuseof.com/beginners-guide-metasploit-kali-linux/)

## 4\. Disable Default Port

We know that MySQL services run on port 3306, and attackers scan the ports to check services running on the network. To add security by obscurity and changes the default MySQL port by editing the port system variable in its main configuration file, you'll need to enter the following:

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT3'); });

 `vim /etc/mysql/my.cnf  
port=XXXX  
sudo service mysql restart` 

## 5\. Avoid Wildcards in Account Names

Account names in MySQL consist of two parts that are a user and a hostname "user\_name"@"host\_name". It enables the administrator to create accounts for the users with the same name who connect from different hosts. However, the host part of an account name permits wildcards conventions that can be a point of access to the database from anywhere.

The optional use of the hostname or IP address value is equivalent to 'user\_name'@'%' where the % matches MySQL pattern matching LIKE operation, and % means any hostname. Meanwhile, access from the '192.168.132.%' means any attempt from the class C network. Besides, anyone can access the database by naming the host part as '192.18.132.mysql.com'.

To avoid such attempts, MySQL allows defining a netmask with the host value to identify the network bits of an IP address:

 `client-ip_add & netmask = host_name` 

The syntax to create a hostname is host\_ip/netmask:

 `CREATE USER 'jhon'@'192.168.132.0/255.255.255.0'; ` 

The above host value enables the user **john** to access the database from any IP within the range of 192.168.132.0-192.168.132.255. Similarly, the host values of 192.168.132.0/255.0.0.0, 192.168.132.0/255.255.0.0 will allow hosts from the class A and B networks. While 192.168.132.5 will only allow access from the specific IP.

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT4'); });

## 6\. Disable Explicit Access

The username in MySQL is either a name with which the databases accept incoming connections or a blank username "@"host\_name" that creates an anonymous user. However, the presence of an anonymous user can leverage attackers to access the database server. Besides, MySQL versions before MySQL 5.7, create an anonymous set of users, and installation after version upgrade still adds these users.

 `select user, host, password from mysql.user where user like '';  
  
` 

  

You can note that the user and password columns are empty, and the access is limited to localhost. However, you do not want anyone to access the database. Use the following command to delete anonymous users:

 `drop user " "@"localhost"  
flush privileges;` 

## 7\. Set Non-Root Account as an Owner or Group

Setting a non-root user account is not related to the MySQL root user. MySQL installation in Linux/Unix systems from tar and tar.gz packages allows the server to run by any underprivileged user. This is a security drawback because any user with the FILE grant option can edit or create files at the server. However, it returns an error when a user tries to access it without the **\-user=root** error.

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT5'); });

You can avoid this by practicing the thumb rule of accessing the database server as a separate Linux user. To run mysqld as a regular Linux user, stop the server and change [read/write permissions](https://www.makeuseof.com/what-are-set-uid-get-uid-and-sticky-bits-in-linux-file-permissions/) of the MySQl server to mysql, as follows:

 `chown -R mysql /path/to/mysql/datadir` 

Open the MySQL main configuration file, add a new mysql user, and restart the service to avoid unrequired server access:

 `vim /etc/mysql/my.cnf  
user=mysql  
sudo service mysql restart` 

## 8\. Set Password for Root Account

MySQL installation via an interactive shell in Debian-based Linux distributions creates the root user account and asks you to set a password. However, this does not happen in non-interactive shell installation and Red-Hat-based distributions. As stated above, a non-root user of a Linux machine can access mysql root user account by using the **\-user=root** option. You can avoid that by setting the password as follows:

 `sudo mysqladmin password  
vim /etc/mysql/my.cnf  
password=<your_password>  
sudo service mysql restart` 

## 9\. Ensure Data Encryption in Transit and at Rest

The default unencrypted communication between the client and the server poses a risk of data interception by any man-in-the-middle. Similarly, unencrypted user data in the database risks the user's confidentiality and integrity. MySQL supports data encryption between the client and the server over TLS/SSL protocol, while unencrypted communication is only acceptable when both communicating parties are within the same network.

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT6'); });

MySQL now supports encryption at rest to protect data stored in the server even when the system is breached.

## MySQL Advanced Security: Protect Yourself

Ensuring that you've got the highest levels of online security is critical, and this article will have given you some useful pointers in the right direction. The above steps are useful for securing your database server, but learning how to assign minimum permissions to non-administrative users is also essential.