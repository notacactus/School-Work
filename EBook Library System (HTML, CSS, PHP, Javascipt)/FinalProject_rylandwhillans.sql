-- phpMyAdmin SQL Dump
-- version 4.7.7
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Jul 30, 2019 at 07:48 AM
-- Server version: 5.7.27
-- PHP Version: 7.1.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `rylandwhillans`
--

-- --------------------------------------------------------

--
-- Table structure for table `proj_authors`
--

CREATE TABLE `proj_authors` (
  `fk_bookid` int(11) NOT NULL,
  `author` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `proj_authors`
--

INSERT INTO `proj_authors` (`fk_bookid`, `author`) VALUES
(18, 'Kyösti Wilkuna'),
(19, 'Charles A. Stearns'),
(22, 'Hendrik Conscience'),
(23, 'James John'),
(23, 'Howard Gregory'),
(24, 'H. B. Fyfe'),
(28, 'Stephen Barr'),
(30, 'Carel Steven'),
(30, 'Adama van Scheltema'),
(32, 'Winston K. Marks'),
(36, 'Jean Rameau'),
(38, 'Kurt Hensel'),
(39, 'Amy Levy'),
(26, 'Victoria Glad'),
(20, 'Kirk Munroe');

-- --------------------------------------------------------

--
-- Table structure for table `proj_bookfiles`
--

CREATE TABLE `proj_bookfiles` (
  `id` int(11) NOT NULL,
  `fk_bookid` int(11) NOT NULL,
  `filename` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `proj_bookfiles`
--

INSERT INTO `proj_bookfiles` (`id`, `fk_bookid`, `filename`) VALUES
(26, 18, 'pg41596.epub'),
(27, 19, 'pg31364.mobi'),
(28, 20, 'pg22497.mobi'),
(29, 20, 'pg22497-images.epub'),
(30, 23, 'pg19006.epub'),
(31, 23, 'pg19006-images.epub'),
(32, 23, 'pg19006-images.mobi'),
(33, 26, 'pg23301.epub'),
(34, 26, 'pg23301.mobi'),
(35, 28, 'pg51330.epub'),
(36, 31, 'pg54338.mobi'),
(37, 34, 'pg52776-images.epub'),
(38, 39, 'pg59990.epub');

-- --------------------------------------------------------

--
-- Table structure for table `proj_books`
--

CREATE TABLE `proj_books` (
  `id` int(11) NOT NULL,
  `fk_userid` int(11) NOT NULL,
  `title` varchar(100) NOT NULL,
  `cover` tinyint(1) NOT NULL,
  `format` varchar(8) DEFAULT NULL,
  `status` varchar(11) DEFAULT NULL,
  `language` varchar(50) DEFAULT NULL,
  `pub_date` date DEFAULT NULL,
  `publisher` varchar(100) DEFAULT NULL,
  `isbn` varchar(50) DEFAULT NULL,
  `extra` varchar(1000) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `proj_books`
--

INSERT INTO `proj_books` (`id`, `fk_userid`, `title`, `cover`, `format`, `status`, `language`, `pub_date`, `publisher`, `isbn`, `extra`) VALUES
(18, 1, 'Aamun miehiä: Historiallinen kuvaelma', 0, NULL, NULL, 'Finish', '2012-12-10', NULL, NULL, NULL),
(19, 1, 'B-12\'s Moon Glow', 0, NULL, 'Not Started', 'English', NULL, NULL, NULL, NULL),
(20, 1, 'Cab and Caboose: The Story of a Railroad Boy', 1, NULL, 'Finished', NULL, '2007-09-04', NULL, NULL, NULL),
(21, 1, 'Aan de kust van Malabar', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(22, 1, 'De baanwachter', 0, 'Physical', 'In Progress', 'Dutch', NULL, NULL, NULL, NULL),
(23, 1, 'Cabbages and Cauliflowers: How to Grow Them', 0, 'Ebook', NULL, NULL, NULL, NULL, NULL, 'Mmm, Cabbages'),
(24, 1, 'D-99', 1, 'Physical', NULL, NULL, NULL, NULL, NULL, NULL),
(25, 1, 'Dab Kinzer: A Story of a Growing Boy', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(26, 1, 'Each Man Kills', 0, 'Physical', NULL, NULL, NULL, NULL, NULL, NULL),
(27, 1, 'Fabeln und Erzählungen', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(28, 1, 'I Am a Nucleus', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(29, 1, 'Mabel: A Novel. Vol. 1 (of 3)', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(30, 1, 'Naakt model: Toneelspel in drie bedrijven', 1, 'Physical', NULL, 'Dutch', NULL, NULL, NULL, NULL),
(31, 1, 'Q-Ships and Their Story', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(32, 1, 'Tabby', 1, 'Physical', 'Not Started', NULL, NULL, NULL, NULL, NULL),
(33, 1, 'The Wages of Virtue', 1, 'Physical', 'Finished', NULL, NULL, NULL, NULL, NULL),
(34, 1, 'X Marks the Pedwalk', 1, 'Ebook', 'Not Started', NULL, NULL, NULL, NULL, NULL),
(35, 1, 'Yachting, Vol. 1', 1, NULL, 'Finished', 'English', NULL, NULL, NULL, NULL),
(36, 1, 'Yan', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(37, 1, 'Z', 1, 'Physical', 'Finished', NULL, NULL, NULL, NULL, NULL),
(38, 1, 'Zahlentheorie', 0, 'Audio', 'In Progress', NULL, NULL, NULL, NULL, NULL),
(39, 1, 'Miss Meredith', 1, 'Ebook', NULL, 'English', '2019-07-27', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `proj_genres`
--

CREATE TABLE `proj_genres` (
  `fk_bookid` int(11) NOT NULL,
  `genre` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `proj_genres`
--

INSERT INTO `proj_genres` (`fk_bookid`, `genre`) VALUES
(19, 'Science Fiction'),
(22, 'Fiction'),
(30, 'Drama'),
(35, 'Recreation'),
(26, 'Horror'),
(26, 'Short Stories');

-- --------------------------------------------------------

--
-- Table structure for table `proj_persistent_sessions`
--

CREATE TABLE `proj_persistent_sessions` (
  `id` int(11) NOT NULL,
  `fk_userid` int(11) NOT NULL,
  `token` varchar(40) NOT NULL,
  `timeout` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `proj_persistent_sessions`
--

INSERT INTO `proj_persistent_sessions` (`id`, `fk_userid`, `token`, `timeout`) VALUES
(2, 1, '11ad96f8a4be4bd91f511659a0d420710218448a', 1563306111),
(4, 1, '190b8b6c9f12eed0571aa530c3cfd0188d5dab4a', 1564952522);

-- --------------------------------------------------------

--
-- Table structure for table `proj_reset_requests`
--

CREATE TABLE `proj_reset_requests` (
  `id` int(11) NOT NULL,
  `fk_userid` int(11) NOT NULL,
  `token` varchar(40) NOT NULL,
  `timeout` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `proj_reset_requests`
--

INSERT INTO `proj_reset_requests` (`id`, `fk_userid`, `token`, `timeout`) VALUES
(2, 1, '5f10758c9e61b49de3741008906ec9d5c44f40d2', 1564388629),
(10, 3, '6f990eadb678a28d05b8c46e14163aa48b1c6e7f', 1564461110),
(11, 5, '4a7c112f45f2d125f11767e4aa92d2750051357e', 1564468958);

-- --------------------------------------------------------

--
-- Table structure for table `proj_users`
--

CREATE TABLE `proj_users` (
  `id` int(11) NOT NULL,
  `email` varchar(254) NOT NULL,
  `passhash` char(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `proj_users`
--

INSERT INTO `proj_users` (`id`, `email`, `passhash`) VALUES
(1, 'toast@binkmail.com', '$2y$10$m7qvjNk0HPsfVKR8mJcVk.iyzZ6ObDrjXW2FmSRvovN7cGVZgWasK'),
(3, 'toast2@binkmail.com', '$2y$10$2Fnw6lkpbphgI8ABe/QjYehA7cDRugByZP69o5RHSD5Pk5MxDDmg.'),
(5, 'toast3@binkmail.com', '$2y$10$DGB2qKjtT3TIFgAMW98bPuAmB16l9glcxq836RGQSgy2FS19iylK6');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `proj_authors`
--
ALTER TABLE `proj_authors`
  ADD KEY `fk_bookid` (`fk_bookid`);

--
-- Indexes for table `proj_bookfiles`
--
ALTER TABLE `proj_bookfiles`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_bookid` (`fk_bookid`);

--
-- Indexes for table `proj_books`
--
ALTER TABLE `proj_books`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_userid` (`fk_userid`);

--
-- Indexes for table `proj_genres`
--
ALTER TABLE `proj_genres`
  ADD KEY `fk_bookid` (`fk_bookid`);

--
-- Indexes for table `proj_persistent_sessions`
--
ALTER TABLE `proj_persistent_sessions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_userid` (`fk_userid`);

--
-- Indexes for table `proj_reset_requests`
--
ALTER TABLE `proj_reset_requests`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_userid` (`fk_userid`);

--
-- Indexes for table `proj_users`
--
ALTER TABLE `proj_users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `proj_bookfiles`
--
ALTER TABLE `proj_bookfiles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT for table `proj_books`
--
ALTER TABLE `proj_books`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=45;

--
-- AUTO_INCREMENT for table `proj_persistent_sessions`
--
ALTER TABLE `proj_persistent_sessions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `proj_reset_requests`
--
ALTER TABLE `proj_reset_requests`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT for table `proj_users`
--
ALTER TABLE `proj_users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `proj_authors`
--
ALTER TABLE `proj_authors`
  ADD CONSTRAINT `proj_authors_ibfk_1` FOREIGN KEY (`fk_bookid`) REFERENCES `proj_books` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `proj_bookfiles`
--
ALTER TABLE `proj_bookfiles`
  ADD CONSTRAINT `proj_bookfiles_ibfk_1` FOREIGN KEY (`fk_bookid`) REFERENCES `proj_books` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `proj_books`
--
ALTER TABLE `proj_books`
  ADD CONSTRAINT `proj_books_ibfk_1` FOREIGN KEY (`fk_userid`) REFERENCES `proj_users` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `proj_genres`
--
ALTER TABLE `proj_genres`
  ADD CONSTRAINT `proj_genres_ibfk_1` FOREIGN KEY (`fk_bookid`) REFERENCES `proj_books` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `proj_persistent_sessions`
--
ALTER TABLE `proj_persistent_sessions`
  ADD CONSTRAINT `proj_persistent_sessions_ibfk_1` FOREIGN KEY (`fk_userid`) REFERENCES `proj_users` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `proj_reset_requests`
--
ALTER TABLE `proj_reset_requests`
  ADD CONSTRAINT `proj_reset_requests_ibfk_1` FOREIGN KEY (`fk_userid`) REFERENCES `proj_users` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
