# Load Data ####

# Install pacman ("package manager") if needed
if (!require("pacman")) install.packages("pacman")

# Load contributed packages with pacman
pacman::p_load(pacman, tidyverse, datasets)

# Import CSV files with readr::read_csv() from tidyverse
(data <- read_csv("Data/Data.csv"))


# Operations ####

data[[ "EI"  ]][1]

#data[[ "ValA"  ]]

#qplot(ValA, data = data)

#qplot(Petal.Length, geom = "histogram", data = iris)

#g <- data %>% ggplot(aes(ValA))
#g + geom_bar()

# Clean Up ####

# Clear environment
rm(list = ls()) 

# Clear packages
p_unload(all)

# Clear plots
graphics.off()

# Clear console
cat("\014")

