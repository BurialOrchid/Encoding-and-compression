Encoding and compression

Repo for coding exercises for encoding and compression subject. 
Sixth semester of classes at the university. 
Uniwersytet Opolski 2020

Language of choice C#

Most of exercises done with .Net Core console projects
Exercises form list 5 done with .Net Core WPF App
**.Net Core version 3.1**

# Exercises to do 
### * List 1 Exercise 9
Write a program that creates a probabilistic model for a text file counting
the number of occurrences of individual letters.

### * List 1 Exercise 10
Write a program that calculates entropy for a file in a probabilistic model with
previous task.

### * List 2 Exercise 4
Write a program that selects letters from the Polish alphabet and creates 200 four-letter words from them in terms of the way:
a) random selection with equal probabilities,
b) on the basis of the attached file 4wyrazy.txt u download the probabilistic model and
	generate words based on this model,
(extra)c) repeat point (b) applies a uniform context for the second, third and fourth
	letters and probabilistic model for the first letter,
(extra)d) repeat point (b) use the two-letter context for the third and fourth letters,
	single letter context for the second letter and probabilistic model for the first
	letters.

### * List 3 Exercise 5 (extra)
Write the program:
a) finding the Huffman code for the symbols appearing in the selected file
	and calculating the average length of this code and comparing it to entropy
	(code redundancy), check for differences in distribution
	probabilities in relation to the distribution taken into account at
	Code constructions affect file compression (bit average)
b) compressing and decompressing any file using encoding
	Huffman (used to compress and decompress Huffman code can
	save in another file),
c) compressing and decompressing any file using dynamic
	Huffman coding

### * List 4 Exercise 4
Write the program:
a) finding a marker from the range [0, 1) for any short sequence
	ASCII and decoding this tag,
(extra)b) implementing any variation of arithmetic coding
	generating binary code and decoding incrementally
	generated code.

### * List 5 Exercise 3
Write a program that:
a) displays a picture saved in the file:
	"Flower_640x500_8bit_Grayscale.raw" in .raw format, i.e. as a string
	bytes, where each byte is the shade of gray of the next pixel in
	image line by line (image size W x H: 640 x 500),
b) sets the table of differences (prediction) between neighboring pixels of the image
	from point a.,
c) display image from pixel difference table (write program so that the alphabet for
	the calculated differences also had 256 symbols),
d) restores the original image from the difference table.
e) encode original image and prediction image with Huffman encoder and
	compare the compression ratio.

### * List 5 Exercise 4 (extra)
Write a program that creates tables of differences (separately for R, G and B) for the image
colored and encodes the processed image with Huffman encoder. Compare the degree
compression of the original image and the image processed by calculation
differences. Recreate the original image from the difference tables.

### * List 5 Exercise 5 (extra)
Instead of a simple sequence of differences, use another selected prediction scheme with
JPEG-LS encoding and use in task 3 or 4.

### * List 6 Exercise 2
Write a program that uses the digram encoding algorithm for
encode text files.
(extra) Decode text files. Analyze the impact of size
dictionary and size of compressed files per degree of compression. Apply
encoder for files containing other text than the one it stayed for
created dictionary - how it affected the obtained compression.

### * List 6 Exercise 3 (extra)
Write a program that displays the image in progressive transmission e.g. by clicking
the next image approximation is displayed in the button. Experiment with
various ways to generate image approximation in transmission
progressive (you can, for example, calculate the average for pixels within a block).

Instead of a simple sequence of differences, use another selected prediction scheme with
JPEG-LS encoding and use in task 3 or 4.