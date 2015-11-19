# R studio download is here: 
#  https://www.rstudio.com/products/rstudio/download/

# Read CSV into R
csv_data <- read.csv(file="./data/1447970910659.57_1001_mouse_screenspace_select_selections.csv", header=TRUE, sep=",")

data = as.data.frame( csv_data )

# make sure some things are loaded as factors (categorical variables)
data$participant   = as.factor( data$participant   )
data$input_method  = as.factor( data$input_method  )
data$interface     = as.factor( data$interface     )
data$task          = as.factor( data$task          )
data$target_number = as.factor( data$target_number )
data$was_selection_correct =
                     as.factor( data$was_selection_correct )

# we want to look at input and interface together, for now
data$input_and_interface <- with(data, interaction(input_method,  interface), drop = TRUE )

# exploratory stuff
# show means and variance
aggregate( data$elapsed, by=list(data$input_and_interface), FUN=function(x) c(mean=mean(x),var=var(x)))
# boxblot of same
boxplot(   elapsed~input_and_interface, data=data  )


# the actual ANOVA
# We're doing a single-factor within-subjects (or, repeated-measures) ANOVA.
# Pretty good explination here:
# http://ww2.coastal.edu/kingw/statistics/R-tutorials/repeated.html
# (though you can skip past all the trouble with loading / shaping the data frame)

# the ANOVA
aov_results = aov( elapsed ~ input_and_interface + Error(input_and_interface/participant), data=data )
print ( aov_results )

# and pairwise t-tests for post-hoc 
#  (I'm not convinced these are the best to do)
with(data, pairwise.t.test(elapsed, input_and_interface,
                           p.adjust.method="holm", 
                           paired=T
    )                     )
    