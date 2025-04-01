# Load Data ####

# Install pacman ("package manager") if needed
if (!require("pacman")) install.packages("pacman")

# Load contributed packages
pacman::p_load(pacman, tidyverse)

# Import CSV files
data <- read_csv("Data/Data.csv")

# Operations ####

## Extraverted vs Introverted ####

# Set Graph Data
g <- data %>% ggplot(aes(EI))

# Customize Graph
g + geom_bar() +
  labs(
    title    = "Extraverted vs Introverted", 
    y        = "Number of Occurrences",
    x        = NULL)


# Save Plot
ggsave("Data/EIChart.png",
       width = 12, height = 6, dpi = 300)

## Sense vs Intuition ####

# Set Graph Data
g <- data %>% ggplot(aes(EI))

# Customize Graph
g + geom_bar() +
  labs(
    title    = "Sense vs Intuition", 
    y        = "Number of Occurrences",
    x        = NULL)


# Save Plot
ggsave("Data/SNChart.png",
       width = 12, height = 6, dpi = 300)

## Thinking vs Feeling ####

# Set Graph Data
g <- data %>% ggplot(aes(EI))

# Customize Graph
g + geom_bar() +
  labs(
    title    = "Thinking vs Feeling", 
    y        = "Number of Occurrences",
    x        = NULL)


# Save Plot
ggsave("Data/TFChart.png",
       width = 12, height = 6, dpi = 300)

## Judging vs Perceiving ####

# Set Graph Data
g <- data %>% ggplot(aes(EI))

# Customize Graph
g + geom_bar() +
  labs(
    title    = "Judging vs Perceiving", 
    y        = "Number of Occurrences",
    x        = NULL)


# Save Plot
ggsave("Data/JPChart.png",
       width = 12, height = 6, dpi = 300)

# Clean Up ####

# Clear environment
rm(list = ls()) 

# Clear packages
p_unload(all)

# Clear plots
graphics.off()

# Clear console
cat("\014")

