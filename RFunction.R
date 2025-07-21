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

# Python K-Means ####

# # Set K
# kVal = 3
# 
# # Generate blobs
# data, target = make_blobs(n_samples=300, random_state=10001, centers=3, cluster_std=1.0)
# 
# # Randomly initalize centers
# centers = (np.random.rand(kVal,2) - 0.5) * 20
# 
# # Assign all points to the first cluster
# prevClust = np.zeros(data.shape[0])
# currClust = np.zeros(data.shape[0])
# 
# # Loop variable
# clustDiff = True
# 
# while(clustDiff):
#   # Assign each data point to the nearest center (Manhattan Distance)
#   for i in range(data.shape[0]):
#   clustDist = np.zeros(kVal)
# 
# for j in range(kVal):
#   clustDist[j] = abs(centers[j, 0] - data[i, 0]) + abs(centers[j, 1] - data[i, 1])
# 
# currClust[i] = clustDist.argmin()
# 
# # Adjust centers
# z = []
# for i in range(kVal):
#   z = np.equal(currClust, (i * np.ones(data.shape[0])))
# 
# if(np.sum(z) != 0):
#   centers[i, 0] = np.mean(data[:, 0], where=z)
# centers[i, 1] = np.mean(data[:, 1], where=z)
# 
# # Check for change in clusters
# clustDiff = np.array_equal(currClust, prevClust) != True
# 
# # Set previous clusters to the current clusters
# prevClust = np.copy(currClust)
# 
# # Print final centers
# for i in range(kVal):
#   print("Center", i, "-", centers[i])
# 
# # Plot centers
# plt.scatter(data[:, 0], data[:, 1], s=50);
# plt.scatter(centers[:, 0], centers[:, 1], s=50, c='r');
# plt.show()



# Clean Up ####

# Clear environment
rm(list = ls()) 

# Clear packages
p_unload(all)

# Clear plots
graphics.off()

# Clear console
cat("\014")

