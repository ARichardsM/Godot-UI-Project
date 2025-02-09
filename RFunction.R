# Load Data ####

# Install pacman ("package manager") if needed
if (!require("pacman")) install.packages("pacman")

# Load contributed packages with pacman
pacman::p_load(pacman, party, rio, tidyverse)

# Import CSV files with readr::read_csv() from tidyverse
(data <- read_csv("Data.csv"))


# Operations ####
data[[ "Name"  ]][1]

data[[ "ValA"  ]]

# Clean Up ####

# Clear environment
rm(list = ls()) 

# Clear packages
p_unload(all)

# Clear plots
graphics.off()

# Clear console
cat("\014")