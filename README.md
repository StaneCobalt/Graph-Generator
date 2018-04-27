# GraphGenerator
### by Stephen Sladek

This program provides an example of how you can use R.Net to create a graph from SQL data in C#.

In a nutshell, it takes in info from a database (a very simple one for this example), loads that data into R, generates a chart, then saves that chart as a png file.

The inspiration for this project came from when a colleague of mine wanted to be able to display charts on the web. These charts need to be able to be updated by various people, and pull data from SQL. They also need to be able to create charts quickly, which is the reason for my choice of R rather than the built-in chart control. This is just an example that was built in a windows form, but I've placed it here for reference as the code is much the same as could be used for web forms.

Dependencies include:
 - R
 - RStudio for Windows
 - RDotNet Community
 - dplyr
 - ggplot2
 - ggthemes

### Sample Graph
<img src="https://github.com/StaneCobalt/Graph-Generator/blob/master/plot0.png" width="50%"/>
