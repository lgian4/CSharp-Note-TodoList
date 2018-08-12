-- phpMyAdmin SQL Dump
-- version 4.1.14
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Aug 04, 2018 at 04:55 AM
-- Server version: 5.6.17
-- PHP Version: 5.5.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `todo`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddCategory`(IN `_name` VARCHAR(255))
    NO SQL
begin
insert into category(id, name) values (null , _name);
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `AddTask`(IN `_title` VARCHAR(100), IN `_details` VARCHAR(255), IN `_priority` ENUM('none','urgent'), IN `_category` VARCHAR(255))
begin
declare category_id int(5);

select id into @category_id from todo.category where name = _category;

START TRANSACTION;
 
IF @category_id is null THEN
	INSERT INTO category(`name`) VALUES (_category);
	SELECT LAST_INSERT_ID() INTO @category_id; 
    
END IF;
COMMIT;	
START TRANSACTION;
	insert into task(`title`,`details`,`priority`,`category_id`,`modified_date`) values(_title, _details, _priority, @category_id, now());

COMMIT;  


END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteCategory`(IN `_categoryId` INT)
    NO SQL
delete from category where id = _categoryId$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteTask`(IN `_id` INT)
    NO SQL
begin
delete from task where id = _id;
end$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllCategory`()
    NO SQL
select * from category$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllCategoryName`()
    NO SQL
select name from category$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllCategoryNameDistinct`()
    NO SQL
select distinct b.name as categoryname  from task as a inner join category as b on a.category_id = b.id where status = 'incomplete' order by a.status asc, a.priority desc,  a.modified_date desc, a.title asc$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetCompleteTask`()
select a.id , a.title, a.details, a.priority, a.status, b.name as categoryname, a.modified_date from task as a inner join category as b on a.category_id = b.id where status = 'complete' order by   a.modified_date desc, a.title asc$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetIncompleteTask`()
select a.id , a.title, a.details, a.priority, a.status, b.name as categoryname, a.modified_date from task as a inner join category as b on a.category_id = b.id where status = 'incomplete' order by a.priority desc,  a.modified_date desc, a.title asc$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetNameCategory`(IN `_categoryId` INT)
    NO SQL
select name from category where id  = _categoryId$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetOneTask`(IN `_id` INT)
    NO SQL
select * from task where id = _id$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetTaskByCategory`(IN `_category` VARCHAR(255))
    NO SQL
select a.id , a.title, a.details, a.priority, a.status, b.name as categoryname, a.modified_date from task as a inner join category as b on a.category_id = b.id where status = 'incomplete' and b.name = _category order by a.status asc, a.priority desc,  a.modified_date desc, a.title asc$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `GetTrashTask`()
    NO SQL
select a.id , a.title, a.details, a.priority, a.status, b.name as categoryname, a.modified_date from task as a inner join category as b on a.category_id = b.id where status = 'delete' order by a.modified_date desc, a.title asc$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateCategory`(IN `_id` INT, IN `_name` INT)
    NO SQL
update category set name = _name where id = _id$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdatePriorityTask`(IN `_id` INT, IN `_priority` ENUM('none','urgent'))
    NO SQL
begin
	
update  task set priority = _priority where id = _id; 

END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateStatusTask`(IN `_id` INT, IN `_status` ENUM('incomplete','complete','delete'))
    NO SQL
begin
update  task set status = _status where id = _id; 
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateTask`(IN `_id` INT, IN `_title` VARCHAR(100), IN `_details` VARCHAR(255), IN `_priority` ENUM('none','urgent'), IN `_status` ENUM('incomplete','complete'), IN `_category` VARCHAR(255))
    NO SQL
begin
declare category_id int(5);
select id into @category_id from todo.category where name = _category;

START TRANSACTION;
 
IF @category_id is null THEN
	INSERT INTO category(`name`) VALUES (_category);
	SELECT LAST_INSERT_ID() INTO @category_id; 
    
END IF;
COMMIT;	
START TRANSACTION;
	update task set title = _title ,details =  _details,priority = _priority, category_id =  @category_id where id = _id;
    

COMMIT;  


END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `category`
--

CREATE TABLE IF NOT EXISTS `category` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=8 ;

--
-- Dumping data for table `category`
--

INSERT INTO `category` (`id`, `name`) VALUES
(1, 'Untitled'),
(2, 'School'),
(3, 'Job'),
(4, 'Store'),
(5, 'Sadfasdf'),
(6, 'Asdfas'),
(7, 'Example');

-- --------------------------------------------------------

--
-- Table structure for table `task`
--

CREATE TABLE IF NOT EXISTS `task` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(100) NOT NULL,
  `details` varchar(255) DEFAULT NULL,
  `priority` enum('none','urgent') NOT NULL DEFAULT 'none',
  `status` enum('incomplete','complete','delete') NOT NULL DEFAULT 'incomplete',
  `category_id` int(11) NOT NULL,
  `modified_date` datetime NOT NULL,
  PRIMARY KEY (`id`),
  KEY `category_id` (`category_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=11 ;

--
-- Dumping data for table `task`
--

INSERT INTO `task` (`id`, `title`, `details`, `priority`, `status`, `category_id`, `modified_date`) VALUES
(2, 'Simple Task', 'Simple details', 'none', 'complete', 4, '2018-08-04 09:38:14'),
(3, 'Sample task', 'Job', 'none', 'incomplete', 3, '2018-08-04 09:38:27'),
(8, 'Note', 'Note example', 'urgent', 'incomplete', 2, '2018-08-04 09:50:42'),
(10, 'Hgf', 'Fjhfgq', 'none', 'delete', 7, '2018-08-04 09:43:06');

--
-- Triggers `task`
--
DROP TRIGGER IF EXISTS `updateTask`;
DELIMITER //
CREATE TRIGGER `updateTask` BEFORE UPDATE ON `task`
 FOR EACH ROW begin
set new.modified_date = now();
end
//
DELIMITER ;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `task`
--
ALTER TABLE `task`
  ADD CONSTRAINT `task_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `category` (`id`) ON DELETE NO ACTION ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
