# LzwGifTools v0.1-alpha
A C# LZW GIF compression library for .NET 4.5

### A brief introduction
So you've got a GIF file and you want to mess around with the image data. Well, the [gif89a spec](https://www.w3.org/Graphics/GIF/spec-gif89a.txt) mandates that GIF image data is compressed once using a special variation of [Lempel-Ziv-Welch](https://en.wikipedia.org/wiki/Lempel%E2%80%93Ziv%E2%80%93Welch) encoding (hereby referred to as LZW GIF), and compressed *again* using [variable-length encoding](https://en.wikipedia.org/wiki/Variable-width_encoding). Now you're doomed to months of scouring byzantine corners of the internet to unlock the secrets of once-living data scientists and mathematicians.  Until now!

LzwGifTools is here to help you with tasks such as:
* Decoding variable-width encoded bytestreams to extract LZW-compressed image data
* Decompressing LZW code streams into tangible GIF bitmap data
* Compressing GIF bitmap data into LZW code streams
* Encoding LZW code streams back into ugly, obfuscated GIF image data

### FAQ
* *What does it mean for data to be "LZW-compressed"?*

Roughly speaking, Lempel–Ziv–Welch compression involves taking some stream of data, looking for patterns in the data, storing pattens of data in a dictionary as LZW codes, and emitting a new stream of encoded data.

Consider the following stream of 40 integers:

```
100 200 100 200 100 200 100 200 100 200 100 200 100 200 100 200 100 200 100 200
100 200 100 200 100 200 100 200 100 200 100 200 100 200 100 200 100 200 100 200
```

The LZW algorithm basically starts by building some sort of basic dictionary based on the data elements in the stream, for instance:

| Index  | Value   |
| ------:|--------:|
| 0      | 100     |
| 1      | 200     |

And uncovers patterns in the data, looking for new sequences to add to the stream:

| Index  | Value   |
| ------:|:--------|
| 2      | 100 200                         |
| 3      | 100 200 100 200                 |
| 4      | 100 200 100 200 100 200 100 200 |

Once a sufficiently large dictionary has been constructed, a new LZW code stream can be constructed based off of the elements in the dictionary:

```
4 4 4 4 4
```

And now you've got an LZW-compressed code stream!

* *What about LZW decompression?*

The basic idea is that with a stream of LZW codes and an initial dictionary, the same dictionary constructed during the *encoding* process can be reconstructed during the *decoding* process.  Then the dictionary can be used to look up LZW codes and get the original sequence back.

* *What's LZW GIF?*

LZW GIF adds a few special codes to the dictionary, such as an "End of Information" code and a "Clear" code.  And there's some special procedures when you've added a certain amount of codes to the dictionary - for instance, LZW GIF dictionary codes can't exceed 4095 (2<sup>12</sup>-1) - so "Clear" codes are used to re-intialize the dictionary whenever the dictionary is about to exceed this capacity.

* *How about variable-width encoding?*

Going back to the 40 integer stream example above - each integer in C# is expressed in 32-bits, but each of the integers in *our* stream need only be expressed using 8 bits. Very roughly speaking: variable-width encoding cuts off the extraneous 24 bits of each integer in our data stream.

If we've got a stream of LZW codes, the codes will generally start around 0-1 (1-bit), 2-3 (2-bits), 4-7 (3-bits), 8-15 (4 bits), and so on.  We can start encoding codes using only one bit, but eventually we'd have to start writing codes using 2 bits, 3 bits, and so on and so forth.  In other words, the code width varies, hence the *varible-width* bit.

* *How does a variable-width decoder know when to start looking at every 3 bits instead of 2?*

LZW codes increase at an exponential rate - every 2<sup>N</sup>-1 LZW codes (where N represents the "code width", or the amount of bits required to express a single LZW dictionay code), the code width should increase by one, to a maximum of 12 bits.  So if you start by looking at a bytestream 2 bits at a time, you should start looking at three bit after you've looked at 2<sup>2</sup>-1 codes.

### Special thanks
Special thanks to Matthew Flickinger for his [What's in a GIF? tutorial](http://www.matthewflickinger.com/lab/whatsinagif/lzw_image_data.asp), Joe Bowers for his awesome [python-lzw](https://github.com/joeatwork/python-lzw) library (where some bit-twiddling ideas have been gleaned), and the elusive [greybeard](http://stackoverflow.com/users/3789665/greybeard).
