-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 13, 2025 at 06:01 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `cms_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_contact`
--

CREATE TABLE `tbl_contact` (
  `id` int(11) NOT NULL,
  `uid` int(11) DEFAULT NULL,
  `con_message` varchar(10000) DEFAULT NULL,
  `con_datetime` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_contact`
--

INSERT INTO `tbl_contact` (`id`, `uid`, `con_message`, `con_datetime`) VALUES
(6, 19, 'testing', '2/13/2025');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_zoomappointments`
--

CREATE TABLE `tbl_zoomappointments` (
  `id` int(11) NOT NULL,
  `uid` int(11) NOT NULL,
  `AppointmentDateTime` varchar(25) NOT NULL,
  `applink` varchar(1000) NOT NULL,
  `Status` varchar(50) NOT NULL,
  `IpAddress` varchar(16) NOT NULL,
  `ClientLocation` varchar(1000) NOT NULL,
  `ClientLat` varchar(50) NOT NULL,
  `ClientLon` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tbl_zoomappointments`
--

INSERT INTO `tbl_zoomappointments` (`id`, `uid`, `AppointmentDateTime`, `applink`, `Status`, `IpAddress`, `ClientLocation`, `ClientLat`, `ClientLon`) VALUES
(7, 21, '2/14/2025 8:00:00 PM', 'https://zoom.us/j/92674307997?pwd=MBl9NPWj6Q6DmiW6vyVarXoSbTZwsn.1', 'New Appointment', '110.38.230.2', 'Karachi Sindh, Pakistan. Postal code:74000', '24.8608', '67.0104'),
(8, 21, '2/14/2025 2:30:00 AM', 'https://zoom.us/j/99840809846?pwd=3d2pPDLla0UFCJ0i2sbtW5jlmnyBew.1', 'New Appointment', '110.38.230.2', 'Karachi Sindh, Pakistan. Postal code:74000', '24.8608', '67.0104'),
(9, 19, '2/14/2025 8:30:00 PM', 'https://zoom.us/j/92795633310?pwd=5Amw4ZH19xBP4ukDvbaDpnozh4MqdM.1', 'New Appointment', '110.38.230.2', 'Karachi Sindh, Pakistan. Postal code:74000', '24.8608', '67.0104'),
(10, 19, '2/14/2025 9:00:00 PM', 'https://zoom.us/j/96168641494?pwd=SbKvIOzhsWXcbk6zzBx8YDzTmLrUby.1', 'New Appointment', '110.38.230.2', 'Karachi Sindh, Pakistan. Postal code:74000', '24.8608', '67.0104');

-- --------------------------------------------------------

--
-- Table structure for table `useraccounts`
--

CREATE TABLE `useraccounts` (
  `id` int(11) NOT NULL,
  `designation` varchar(256) NOT NULL,
  `organization` varchar(512) NOT NULL,
  `firstName` varchar(50) NOT NULL,
  `lastName` varchar(255) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  `phone` varchar(15) DEFAULT NULL,
  `gender` varchar(6) NOT NULL,
  `roleName` varchar(50) DEFAULT NULL,
  `profileImage` varchar(256) DEFAULT NULL,
  `createdDate` varchar(25) DEFAULT NULL,
  `status` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `useraccounts`
--

INSERT INTO `useraccounts` (`id`, `designation`, `organization`, `firstName`, `lastName`, `email`, `password`, `phone`, `gender`, `roleName`, `profileImage`, `createdDate`, `status`) VALUES
(5, 'Manager', 'Intersys Private Limited', 'Noman', 'Khan', 'nomankhan@gmail.com', '123', '+92334216000', 'Male', 'Admin', 'nomankhan@gmail.com.png', '1/28/2025', 1),
(10, 'Voice President', 'Intersys Private Limited', 'Majid', 'Ali', 'majidali@gmail.com', '123', '+92334216002', 'Male', 'Admin', 'majidali@gmail.com.png', '1/28/2025', 1),
(19, '', '', 'Muhammad', 'Qasim', 'qasim_mscs@live.com', '', '+923342160799', '', 'Contact', NULL, '2/10/2025', 1),
(20, '', '', 'hassan', '', 'hassan.alihabb@gmail.com', '', '+923133698588', '', 'Client', NULL, '2/10/2025', 1),
(21, '', '', 'qasim', '', 'muhammad.qasim1@intersyslimited.org', '', '+92 334 2160799', '', 'Client', NULL, '2/11/2025', 1);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_contact`
--
ALTER TABLE `tbl_contact`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `tbl_zoomappointments`
--
ALTER TABLE `tbl_zoomappointments`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `useraccounts`
--
ALTER TABLE `useraccounts`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_contact`
--
ALTER TABLE `tbl_contact`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tbl_zoomappointments`
--
ALTER TABLE `tbl_zoomappointments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `useraccounts`
--
ALTER TABLE `useraccounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
