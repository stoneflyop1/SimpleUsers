-- https://www.ntu.edu.sg/home/ehchua/programming/sql/MySQL_Intermediate.html

-- set privileges for all hosts
-- https://stackoverflow.com/questions/35886949/docker-granting-access-to-linked-mysql-container
GRANT ALL PRIVILEGES ON *.* TO 'root'@'%';
FLUSH PRIVILEGES;

CREATE DATABASE dotnetusers DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE dotnetusers;

CREATE TABLE IF NOT EXISTS `User` (
    `Id` VARCHAR(36) NOT NULL,
    `UserName` VARCHAR(255) NOT NULL,
    `PasswordHash` NVARCHAR(255) NOT NULL,
    `Name` NVARCHAR(255),
    `Mobile` VARCHAR(255),
    `Email` NVARCHAR(255),
    PRIMARY KEY  (Id)
) ENGINE=InnoDB DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;