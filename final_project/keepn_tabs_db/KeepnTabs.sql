# ************************************************************
# Sequel Pro SQL dump
# Version 4541
#
# http://www.sequelpro.com/
# https://github.com/sequelpro/sequelpro
#
# Host: 127.0.0.1 (MySQL 5.7.25)
# Database: KeepnTabs
# Generation Time: 2019-04-14 04:10:26 +0000
# ************************************************************


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


# Dump of table Lists
# ------------------------------------------------------------

DROP TABLE IF EXISTS `Lists`;

CREATE TABLE `Lists` (
  `ID` varchar(36) NOT NULL DEFAULT '',
  `Title` varchar(256) NOT NULL DEFAULT '',
  `UserID` varchar(36) NOT NULL DEFAULT '',
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `lists_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `Users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



# Dump of table Tasks
# ------------------------------------------------------------

DROP TABLE IF EXISTS `Tasks`;

CREATE TABLE `Tasks` (
  `ID` varchar(36) NOT NULL DEFAULT '',
  `Title` varchar(256) NOT NULL DEFAULT '',
  `Done` tinyint(1) NOT NULL,
  `ListID` varchar(36) NOT NULL DEFAULT '',
  PRIMARY KEY (`ID`),
  KEY `ListID` (`ListID`),
  CONSTRAINT `tasks_ibfk_1` FOREIGN KEY (`ListID`) REFERENCES `Lists` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



# Dump of table Tokens
# ------------------------------------------------------------

DROP TABLE IF EXISTS `Tokens`;

CREATE TABLE `Tokens` (
  `ID` varchar(36) NOT NULL DEFAULT '',
  `UserID` varchar(36) NOT NULL DEFAULT '',
  `Expires` datetime NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `tokens_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `Users` (`ID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



# Dump of table Users
# ------------------------------------------------------------

DROP TABLE IF EXISTS `Users`;

CREATE TABLE `Users` (
  `ID` varchar(36) NOT NULL DEFAULT '',
  `Email` varchar(320) NOT NULL DEFAULT '',
  `Pass` varchar(256) NOT NULL DEFAULT '',
  `Confirmed` tinyint(4) NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `UniqueEmail` (`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;

INSERT INTO `Users` (`ID`, `Email`, `Pass`, `Confirmed`)
VALUES
	('bfcb24ee-5e4f-11e9-ad13-8dcf6690a175','test@test.com','password',0);

/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;



--
-- Dumping routines (PROCEDURE) for database 'KeepnTabs'
--
DELIMITER ;;

# Dump of PROCEDURE TokenDelete
# ------------------------------------------------------------

/*!50003 DROP PROCEDURE IF EXISTS `TokenDelete` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `TokenDelete`( tk varchar( 36 ) )
begin
	delete from Tokens where ID = tk;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of PROCEDURE TokenUpdate
# ------------------------------------------------------------

/*!50003 DROP PROCEDURE IF EXISTS `TokenUpdate` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `TokenUpdate`( tk varchar( 36 ) )
begin
	declare ts datetime;
	
	set ts = date_add( now(), interval 30 minute );
	
	update Tokens set Expires = ts;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of PROCEDURE UserConfirm
# ------------------------------------------------------------

/*!50003 DROP PROCEDURE IF EXISTS `UserConfirm` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `userconfirm`( u varchar( 36 ) )
begin
	update Users set Confirmed = true where ID = u;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of PROCEDURE UserLogout
# ------------------------------------------------------------

/*!50003 DROP PROCEDURE IF EXISTS `UserLogout` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 PROCEDURE `userlogout`( tk varchar( 36 ) )
begin
	call TokenDelete( tk );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
DELIMITER ;

--
-- Dumping routines (FUNCTION) for database 'KeepnTabs'
--
DELIMITER ;;

# Dump of FUNCTION ListAdd
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `ListAdd` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `ListAdd`( tk varchar( 36 ), t varchar( 256 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare u varchar( 36 );
	declare l varchar( 36 );
	
	select TokenUser( tk ) into u;
	
	if u is not null then
		set l = uuid();
		
		insert into Lists( ID, Title, UserID ) values( l, t, u );
		
		return( l );
	else
		return( u );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION ListDelete
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `ListDelete` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `listdelete`( tk varchar( 36 ), l varchar( 36 ) ) RETURNS tinyint(1)
begin
	declare tu varchar( 36 );
	declare lu varchar( 36 );
	
	select TokenUser( tk ) into tu;
	select ListUser( l )   into lu;
	
	if tu is not null and tu = lu then
		delete from Lists where ID = l;
		
		return( true );
	else
		return( false );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION ListUpdate
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `ListUpdate` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `listupdate`( tk varchar( 36 ), l varchar( 36 ), t varchar( 256 ) ) RETURNS tinyint(1)
begin
	declare tu varchar( 36 );
	declare lu varchar( 36 );
	
	select TokenUser( tk ) into tu;
	select ListUser( l )   into lu;
	
	if tu is not null and tu = lu then
		update Lists set Title = t where ID = l;
		
		return( true );
	else
		return( false );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION ListUser
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `ListUser` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `ListUser`( l varchar( 36 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare u varchar( 36 );
	
	select UserID from Lists where ID = l into u;
	
	return( u );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION TaskAdd
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `TaskAdd` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `taskadd`( tk varchar( 36 ), l varchar( 36 ), t varchar( 256 ), d tinyint ) RETURNS varchar(36) CHARSET utf8
begin
	declare tu varchar( 36 );
	declare lu varchar( 36 );
	declare ti varchar( 36 );
	
	select TokenUser( tk ) into tu;
	select ListUser( l )   into lu;
	
	if tu is not null and tu = lu then
		set ti = uuid();
		
		insert into Tasks( ID, Title, Done, ListID ) values( ti, t, d, l );
		
		return( ti );
	else
		return( u );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION TaskDelete
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `TaskDelete` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `taskdelete`( tk varchar( 36 ), ti varchar( 36 ) ) RETURNS tinyint(1)
begin
	declare tu varchar( 36 );
	declare tl varchar( 36 );
	declare lu varchar( 36 );
	
	select TokenUser( tk ) into tu;
	select TaskList( ti )  into tl;
	select ListUser( tl )  into lu;
	
	if tu is not null and tu = lu then
		delete from tasks where ID = ti;
		
		return( true );
	else
		return( false );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION TaskList
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `TaskList` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `TaskList`( t varchar( 36 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare l varchar( 36 );
	
	select ListID from Tasks where ID = t into l;
	
	return( l );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION TaskUpdate
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `TaskUpdate` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `taskupdate`( tk varchar( 36 ), ti varchar( 36 ), t varchar( 256 ), d tinyint ) RETURNS tinyint(1)
begin
	declare tu varchar( 36 );
	declare tl varchar( 36 );
	declare lu varchar( 36 );
	
	select TokenUser( tk ) into tu;
	select TaskList( ti )  into tl;
	select ListUser( tl )  into lu;
	
	if tu is not null and tu = lu then
		update Tasks set Title = t, Done = d where ID = ti;
		
		return( true );
	else
		return( false );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION TokenAdd
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `TokenAdd` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `TokenAdd`( u varchar( 36 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare tk varchar( 36 );
	declare ts datetime;
	
	set tk = uuid();
	set ts = date_add( now(), interval 30 minute );
	
	insert into Tokens( ID, UserID, Expires ) values( tk, u, ts );
	
	return( tk );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION TokenExists
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `TokenExists` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `TokenExists`( tk varchar( 36 ) ) RETURNS tinyint(1)
begin
	declare tke boolean;
	
	select exists( select ID from Tokens where ID = tk and Expires > now() ) into tke;
	
	return( tke );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION TokenUser
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `TokenUser` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `TokenUser`( tk varchar( 36 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare u varchar( 36 );
	
	select UserID from Tokens where ID = tk and Expires > now() into u;
	
	return( u );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserAdd
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserAdd` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `useradd`( e varchar( 320 ), p varchar( 256 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare u varchar( 36 );
	
	if UserExists( e, p ) then
		set u = null;
	else
		set u = uuid();
		
		insert into Users( ID, Email, Pass, Confirmed ) values( u, e, p, false );
	end if;
	
	return( u );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserDelete
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserDelete` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `userdelete`( tk varchar( 36 ) ) RETURNS tinyint(1)
begin
	declare u varchar( 36 );
	
	select TokenUser( tk ) into u;
	
	if u is not null then
		delete from Users where ID = u;
		
		return( true );
	else
		return( false );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserExists
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserExists` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `UserExists`( e varchar( 320 ), p varchar( 256 ) ) RETURNS tinyint(1)
begin
	declare uid boolean;
	
	select exists( select ID from Users where Email = e and Pass = p ) into uid;
	
	return( uid );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserID
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserID` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `UserID`( e varchar( 320 ) , p varchar( 256 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare u varchar( 36 );
	
	select ID from Users where Email = e and Pass = p into u;
	
	return( u );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserIsConfirmed
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserIsConfirmed` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `UserIsConfirmed`( e varchar( 320 ), p varchar( 256 ) ) RETURNS tinyint(1)
begin
	declare c boolean;
	
	select Confirmed from Users where Email = e and Pass = p into c;
	
	return( c );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserIsLoggedIn
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserIsLoggedIn` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `UserIsLoggedIn`( tk varchar( 36 ) ) RETURNS tinyint(1)
begin
	declare uli boolean;
	
	select( TokenExists( tk ) ) into uli;
	
	return( uli );
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserLogin
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserLogin` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `userlogin`( e varchar( 320 ), p varchar( 256 ) ) RETURNS varchar(36) CHARSET utf8
begin
	declare tk varchar( 36 );
	
	if UserExists( e, p ) and UserIsConfirmed( e, p ) then
		select TokenAdd( UserID( e, p ) ) into tk;
	else
		set tk = null;
	end if;
	
	return tk;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
# Dump of FUNCTION UserUpdate
# ------------------------------------------------------------

/*!50003 DROP FUNCTION IF EXISTS `UserUpdate` */;;
/*!50003 SET SESSION SQL_MODE="ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION"*/;;
/*!50003 CREATE*/ /*!50020 DEFINER=`root`@`localhost`*/ /*!50003 FUNCTION `userupdate`( tk varchar( 36 ), e varchar( 320 ), p varchar( 256 ) ) RETURNS tinyint(1)
begin
	declare u varchar( 36 );
	
	select TokenUser( tk ) into u;
	
	if u is not null then
		update Users set Email = e, Pass = p where ID = u;
		
		return( true );
	else
		return( false );
	end if;
end */;;

/*!50003 SET SESSION SQL_MODE=@OLD_SQL_MODE */;;
DELIMITER ;

/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
