file = open("C:/Users/James/OneDrive/Documents/revolution_essay.txt", "r")
text = file.read()

first = '('
second = ')'
shouldCopy = 0
copy = ""
for i in text:
    if i == first:
        shouldCopy += 1
        continue
    if (i == second) & (shouldCopy > 0): 
        shouldCopy -= 1
        continue
    if shouldCopy == 0:
        copy = copy + i

fileCopy = open("C:/Users/James/OneDrive/Documents/revolution_essay_no_brackets.txt", "w")
fileCopy.write(copy)

file.close()
fileCopy.close()