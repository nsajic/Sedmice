"""
Card Recognition using OpenCV
Code from the blog post 
http://arnab.org/blog/so-i-suck-24-automating-card-games-using-opencv-and-python

Usage: 

  ./card_img.py filename num_cards training_image_filename training_labels_filename num_training_cards

Example:
  ./card_img.py test.JPG 4 train.png train.tsv 56
  
Note: The recognition method is not very robust; please see SIFT / SURF for a good algorithm.  

"""

import sys
import numpy as np
import matplotlib.pyplot as plt
import cv2
sys.path.insert(0, "/usr/local/lib/python2.7/site-packages/") 
from flask import Flask, request, jsonify
from skimage.io import imread
from skimage.measure import label
from skimage.measure import regionprops
app = Flask(__name__)



###############################################################################
# Utility code from 
# http://git.io/vGi60A
# Thanks to author of the sudoku example for the wonderful blog posts!
###############################################################################

def rectify(h):
  h = h.reshape((4,2))
  hnew = np.zeros((4,2),dtype = np.float32)
  add = h.sum(1)
  hnew[0] = h[np.argmin(add)]
  hnew[2] = h[np.argmax(add)]
   
  diff = np.diff(h,axis = 1)
  hnew[1] = h[np.argmin(diff)]
  hnew[3] = h[np.argmax(diff)]

  return hnew

###############################################################################
# Image Matching
###############################################################################
def preprocess(img):
  gray = cv2.cvtColor(img,cv2.COLOR_BGR2GRAY)
  blur = cv2.GaussianBlur(gray,(5,5),2 )
  thresh = cv2.adaptiveThreshold(blur,255,1,1,11,1)
  return thresh
  
def imgdiff(img1,img2):
  img1 = cv2.GaussianBlur(img1,(5,5),5)
  img2 = cv2.GaussianBlur(img2,(5,5),5)    
  diff = cv2.absdiff(img1,img2)  
  diff = cv2.GaussianBlur(diff,(5,5),5)    
  flag, diff = cv2.threshold(diff, 200, 255, cv2.THRESH_BINARY) 
  return np.sum(diff)  

def find_closest_card(training,img):
  features = preprocess(img)
  #print "%s - %s" % (i, labels[i])
  return sorted(training.values(), key=lambda x:imgdiff(x[1],features))[0][0]
  
def draw_contour(image, c, i):
	# compute the center of the contour area and draw a circle
	# representing the center
	M = cv2.moments(c)
	cX = int(M["m10"] / M["m00"])
	cY = int(M["m01"] / M["m00"])
 
	# draw the countour number on the image
	cv2.putText(image, "#{}".format(i + 1), (cX - 20, cY), cv2.FONT_HERSHEY_SIMPLEX,
		1.0, (255, 255, 255), 2)
 
	# return the image with the contour number drawn on it
	return image

def sort_contours(cnts, method="bottom-to-top"):
    	# initialize the reverse flag and sort index
	reverse = False
	i = 0
 
	# handle if we need to sort in reverse
	if method == "right-to-left" or method == "bottom-to-top":
		reverse = True
 
	# handle if we are sorting against the y-coordinate rather than
	# the x-coordinate of the bounding box
	if method == "top-to-bottom" or method == "bottom-to-top":
		i = 1
 
	# construct the list of bounding boxes and sort them from top to
	# bottom
	boundingBoxes = [cv2.boundingRect(c) for c in cnts]
	(cnts, boundingBoxes) = zip(*sorted(zip(cnts, boundingBoxes),
		key=lambda b:b[1][i], reverse=reverse))
 
	# return the list of sorted contours and bounding boxes
	return (cnts, boundingBoxes)

def my_rgb2gray(img):
  img_gray = np.ndarray((img.shape[0], img.shape[1])) 
  img_gray = 0.21*img[:, :, 0] + 0.77*img[:, :, 1] + 0.07*img[:, :, 2]
  img_gray = img_gray.astype('uint8') 
  return img_gray

def num_cards(img):
  img_gray = my_rgb2gray(img)
  #cv2.imshow("e", img_gray)
  img_tr = img_gray > 100
  #plt.imshow(img_tr, 'gray')
  #plt.show()

  labeled_img = label(img_tr) 
  #cv2.imshow("o", labeled_img)
  regions = regionprops(labeled_img)

  numCards = 0
  for region in regions:
      bbox = region.bbox
        
      h = bbox[2] - bbox[0]
      w = bbox[3] - bbox[1]

      if h > 570:
        numCards = numCards + 1
        
  return numCards
   
###############################################################################
# Card Extraction
###############################################################################  
def getCards(im):
      
  gray = cv2.cvtColor(im,cv2.COLOR_BGR2GRAY)
  #plt.imshow(gray, 'gray')
  #plt.show()

  blur = cv2.GaussianBlur(gray,(5,5),0)
  #plt.imshow(blur, 'gray')
  #plt.show()

  flag, thresh = cv2.threshold(blur, 0, 255, cv2.THRESH_BINARY+cv2.THRESH_OTSU ) 
  #plt.imshow(thresh, 'gray')
  #plt.show()
       
  _, contours, _ = cv2.findContours(thresh,cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)

  numCards = num_cards(im)

  contours = sorted(contours, key=cv2.contourArea,reverse=True)[:numCards]  

  orig = im.copy()

  for (i, c) in enumerate(contours):
    orig = draw_contour(orig, c, i)
 
  (cnts, boundingBoxes) = sort_contours(contours)

  for (i, c) in enumerate(cnts):
    orig = draw_contour(im, c, i)

  for card in cnts: 
    
    peri = cv2.arcLength(card,True)
    approx = rectify(cv2.approxPolyDP(card,0.02*peri,True))

    box = np.int0(approx)
    cv2.drawContours(im,[box],0,(255,255,0),6)
    imx = cv2.resize(im,(1000,600))
    #cv2.imshow('a',imx)      
    

    h = np.array([ [0,0],[449,0],[449,449],[0,449] ],np.float32)

    transform = cv2.getPerspectiveTransform(approx,h)
    warp = cv2.warpPerspective(im,transform,(450,450))

    yield warp


def get_training(training_labels_filename,training_image_filename,avoid_cards=None):
  training = {}
  
  labels = {}
  for line in file(training_labels_filename): 
    key, num, suit = line.strip().split()
    print "%d - %s  %s" % (int(key), num, suit)
    labels[int(key)] = (num,suit)
    
  print "Training"

  im = cv2.imread(training_image_filename)

  for i,c in enumerate(getCards(im)):
    if avoid_cards is None or (labels[i][0] not in avoid_cards[0] and labels[i][1] not in avoid_cards[1]):
      training[i] = (labels[i], preprocess(c))

  print "Done training"
  return training
  

@app.route('/lang', methods=['POST'])
def addOne(): 
    if len(sys.argv) == 1:
      filename = "testCard.jpg"
      training_image_filename = "trainSedmiceD.jpg"
      training_labels_filename = "tsvSedmice.tsv"  
      
      training = get_training(training_labels_filename,training_image_filename)

      im = cv2.imread(filename)
      
      width = im.shape[0]
      height = im.shape[1]
      #if width < height:
        #im = cv2.transpose(im)
        #im = cv2.flip(im,1)

      
      # Debug: uncomment to see registered images
      
      for i,c in enumerate(getCards(im)):
        
        image = c
        width = image.shape[0]
        height = image.shape[1]
        if width < height:
          image = cv2.transpose(image)
          image = cv2.flip(image,1)

        card = find_closest_card(training,image,)
 
        
        #cv2.imshow(str(card),image)

      cv2.waitKey(0) 
      
      cards = [find_closest_card(training,c) for c in getCards(im)]
      print cards
      return str(cards)
      
    else:
      print __doc__
      return 'testElse'

if __name__ == '__main__':
    app.run(debug=False, port=8081)