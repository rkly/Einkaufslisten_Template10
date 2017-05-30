-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server Version:               10.1.13-MariaDB - mariadb.org binary distribution
-- Server Betriebssystem:        Win32
-- HeidiSQL Version:             9.3.0.4984
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Exportiere Datenbank Struktur für einkaufsliste
CREATE DATABASE IF NOT EXISTS `einkaufsliste` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `einkaufsliste`;


-- Exportiere Struktur von Tabelle einkaufsliste.einheit
CREATE TABLE IF NOT EXISTS `einheit` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Exportiere Daten aus Tabelle einkaufsliste.einheit: ~0 rows (ungefähr)
DELETE FROM `einheit`;
/*!40000 ALTER TABLE `einheit` DISABLE KEYS */;
/*!40000 ALTER TABLE `einheit` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle einkaufsliste.einkaufsliste
CREATE TABLE IF NOT EXISTS `einkaufsliste` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `user_id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK__user` (`user_id`),
  CONSTRAINT `FK__user` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Exportiere Daten aus Tabelle einkaufsliste.einkaufsliste: ~0 rows (ungefähr)
DELETE FROM `einkaufsliste`;
/*!40000 ALTER TABLE `einkaufsliste` DISABLE KEYS */;
/*!40000 ALTER TABLE `einkaufsliste` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle einkaufsliste.produkt
CREATE TABLE IF NOT EXISTS `produkt` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Exportiere Daten aus Tabelle einkaufsliste.produkt: ~0 rows (ungefähr)
DELETE FROM `produkt`;
/*!40000 ALTER TABLE `produkt` DISABLE KEYS */;
/*!40000 ALTER TABLE `produkt` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle einkaufsliste.produkt_einkaufsliste
CREATE TABLE IF NOT EXISTS `produkt_einkaufsliste` (
  `id_einkaufsliste` int(11) NOT NULL,
  `id_produkt` int(11) NOT NULL,
  `id_einheit` int(11) NOT NULL,
  `menge` int(11) NOT NULL,
  UNIQUE KEY `Schlüssel 1` (`id_einkaufsliste`,`id_produkt`),
  KEY `FK__produkt` (`id_produkt`),
  KEY `FK__einheit` (`id_einheit`),
  CONSTRAINT `FK__einheit` FOREIGN KEY (`id_einheit`) REFERENCES `einheit` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK__einkaufsliste` FOREIGN KEY (`id_einkaufsliste`) REFERENCES `einkaufsliste` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK__produkt` FOREIGN KEY (`id_produkt`) REFERENCES `produkt` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Exportiere Daten aus Tabelle einkaufsliste.produkt_einkaufsliste: ~0 rows (ungefähr)
DELETE FROM `produkt_einkaufsliste`;
/*!40000 ALTER TABLE `produkt_einkaufsliste` DISABLE KEYS */;
/*!40000 ALTER TABLE `produkt_einkaufsliste` ENABLE KEYS */;


-- Exportiere Struktur von Tabelle einkaufsliste.user
CREATE TABLE IF NOT EXISTS `user` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Exportiere Daten aus Tabelle einkaufsliste.user: ~0 rows (ungefähr)
DELETE FROM `user`;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;