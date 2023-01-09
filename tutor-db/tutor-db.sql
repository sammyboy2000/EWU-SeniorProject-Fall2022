CREATE DATABASE IF NOT EXISTS tutor_db;
USE tutor_db;

#
# TABLE STRUCTURE FOR: Users
#

DROP TABLE IF EXISTS `Users`;

CREATE TABLE `Users` (
  `user_id` varchar(255) NOT NULL,
  `external_id` varchar(255) NOT NULL,
  `is_student` BIT(1) NOT NULL,
  `is_tutor` BIT(1) NOT NULL,
  `is_admin` BIT(1) NOT NULL,
  UNIQUE KEY `user_id` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# TABLE STRUCTURE FOR: Student
#

DROP TABLE IF EXISTS `Student`;

CREATE TABLE `Student` (
  `id` varchar(255) NOT NULL,
  `external_id` varchar(255) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(100) NOT NULL,
  UNIQUE KEY `id` (`id`),
  FOREIGN KEY (external_id) REFERENCES Users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# TABLE STRUCTURE FOR: Tutor
#

DROP TABLE IF EXISTS `Tutor`;

CREATE TABLE `Tutor` (
  `id` varchar(255) NOT NULL,
  `external_id` varchar(255) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  KEY `id` (`id`),
  FOREIGN KEY (external_id) REFERENCES Users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# TABLE STRUCTURE FOR: Admin
#

DROP TABLE IF EXISTS `Admin`;

CREATE TABLE `Admin` (
  `id` varchar(255) NOT NULL,
  `external_id` varchar(255) NOT NULL,
  `first_name` varchar(100) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  KEY `id` (`id`),
  FOREIGN KEY (external_id) REFERENCES Users(user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# TABLE STRUCTURE FOR: Class
#

DROP TABLE IF EXISTS `Class`;

CREATE TABLE `Class` (
  `id` int(9) unsigned NOT NULL AUTO_INCREMENT,
  `class_code` varchar(10) NOT NULL,
  UNIQUE KEY `id` (`id`),
  UNIQUE KEY `class_code` (`class_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# TABLE STRUCTURE FOR: Topic
#

DROP TABLE IF EXISTS `Topic`;

CREATE TABLE `Topic` (
  `id` int(9) unsigned NOT NULL AUTO_INCREMENT,
  `class_id` int(9) unsigned NOT NULL,
  `topic` varchar(255) NOT NULL,
  KEY `id` (`id`),
  FOREIGN KEY (class_id) REFERENCES Class(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# TABLE STRUCTURE FOR: Questions
#

DROP TABLE IF EXISTS `Questions`;

CREATE TABLE `Questions` (
  `question_id` varchar(255) NOT NULL,
  `student_id` varchar(255) NOT NULL,
  `class_id` int(9) NOT NULL,
  `topic_id` int(9) NOT NULL,
  `content` varchar(750) NOT NULL,
  UNIQUE KEY `question_id` (`question_id`),
  FOREIGN KEY (topic_id) REFERENCES Topic(id),
  FOREIGN KEY (class_id) REFERENCES Class(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# TABLE STRUCTURE FOR: Answered_Questions
#

DROP TABLE IF EXISTS `Answered_Questions`;

CREATE TABLE `Answered_Questions` (
  `question_id` varchar(255) NOT NULL,
  `student_id` varchar(255) NOT NULL,
  `tutor_id` varchar(255) NOT NULL,
  `class_id` int(9) NOT NULL,
  `topic_id` varchar(255) NOT NULL,
  `content` varchar(750) NOT NULL,
  `response` varchar(1000) NOT NULL,
  UNIQUE KEY `question_id` (`question_id`),
  FOREIGN KEY (topic_id) REFERENCES Topic(id),
  FOREIGN KEY (class_id) REFERENCES Class(id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
