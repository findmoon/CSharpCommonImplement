**使用HTML、CSS 和 JavaScript创建简单的图库**

> [How to Create a Simple Image Gallery Using HTML, CSS, and JavaScript](https://www.makeuseof.com/image-gallery-html-css-javascript/)

Creating a simple image gallery using HTML, CSS, and JavaScript is a great way to learn the basics of web development. In the image gallery, you will be able to flick through images by selecting thumbnails to enlarge the image on the webpage.

To create the gallery, you can use HTML to add the webpage content and CSS to add the styling. You can use JavaScript to make the gallery interactive when the user clicks on any of the thumbnails.

window.addEventListener('DOMContentLoaded', () => { $vvvInit('adsninja-ad-unit-characterCountRepeatable1-5f42160cbd920c', 'MUO\_Video\_Desktop', \['https://video.adsninja.ca/valnetinc/MakeUseOf/640a2aa78f668-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/63f7d3a0273d6-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/63f7d4145084e-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/640a2b63834fa-projectRssVideoFile.mp4', 'https://video.adsninja.ca/valnetinc/MakeUseOf/63f7d368607ff-projectRssVideoFile.mp4'\]) })

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-connectedBelowAd-5f4216389a0bf8'); });

## How to Create the UI for the Image Gallery

Add the UI for the image gallery using HTML and CSS. This includes adding a large image in the center of the webpage, which will change based on the thumbnail selected. You can also view the full example [source code on GitHub](https://github.com/makeuseofcode/js-image-gallery).

1. Create a new file called "index.html".
2. Inside this file, add the basic HTML code structure:
    
     `<!doctype html>  
    <html lang="en-US">   
      <head>   
        <title>Image Gallery</title>   
      </head>   
      <body>   
        <div class="intro">   
          <h2>Image Gallery</h2>   
          <p>Select a thumbnail below to view the image</p>   
        </div>   
      </body>  
    </html>` 
    
3. Create a subfolder called "images". Populate it with several images, and name them from "image1.jpg" to "image10.jpg".
    
      
    
4. In your HTML file, add a div for the image gallery:
    
     `<div id="image-gallery">  
      
    </div>` 
    
5. Inside the image gallery div, add an image tag to display the enlarged selected image. By default, display the first image inside the "images" folder:
    
     `<img id="current-image" src="images/image1.jpg" alt="Image 1">` 
    
6. In the same folder as your HTML file, add a new file called "styles.css" with the following CSS. Feel free to modify the CSS to add [neumorphic design components](https://www.makeuseof.com/5-neumorphic-design-components-using-html-css-and-javascript/) or make other design tweaks using [these CSS tips and tricks](https://www.makeuseof.com/css-tips-and-tricks-you-must-know/).
7.  `.intro {   
      text-align: center;   
      font-family: Arial;  
    }  
      
    h2 {   
      font-size: 36px;  
    }  
      
    p {   
      font-size: 14pt;  
    }  
      
    #image-gallery {   
      width: 600px;   
      margin: 0 auto;  
    }  
      
    #current-image {   
      width: 100%;  
    } ` 
    
8. Add a link to your CSS file in the head tag of your HTML file:
    
     `<link rel="stylesheet" type="text/css" href="styles.css">` 
    

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT2'); });

## How to Add Thumbnails for the Other Images in the Gallery

Currently, the image gallery only displays the first image. Underneath the selected image, add a list of thumbnails. These thumbnails will display a preview of all the images inside the "images" folder.

1. Inside the image gallery div in the HTML file, add another div to display thumbnails for the other images:
    
     `<div id="image-thumbs"></div>` 
    
2. Inside the CSS file, add some styling for the list of thumbnails:
    
     `#image-thumbs {   
      display: flex;   
      justify-content: center;   
      margin-top: 20px;  
    }` 
    
3. In the same folder as your HTML and CSS files, add a new file called "script.js".
4. Add a link to your JavaScript file at the bottom of the HTML body tag:
    
     `<body>   
      <!-- Your code here -->   
      <script src="script.js"></script>  
    </body>` 
    
5. Inside the JavaScript file, get the HTML element of the div that will store the list of thumbnails:
    
     `var imageThumbs = document.getElementById("image-thumbs");` 
    
6. Add a for-loop to loop through each of the 10 images in the gallery:
    
     `for (var i = 1; i <= 10; i++) {  
      
    }` 
    
7. Inside the loop, create a new img element for each image:
    
     `var thumb = document.createElement("img");` 
    
8. Add values for the "src" and "alt" attributes. In this case, the "src" attribute is the file path to the image at the same index inside the "images" folder:
    
     `thumb.src = "images/image" + i + ".jpg";  
    thumb.alt = "Image " + i;` 
    
9. Inside your CSS file, add a new class to style the image's thumbnail. You can also add other hover or transitional CSS styling for the thumbnails to [make your website responsive and interactive](https://www.makeuseof.com/how-to-make-website-responsive/).
    
     `.thumb {   
      width: 80px;   
      height: 80px;   
      object-fit: cover;   
      margin-right: 10px;   
      cursor: pointer;  
    }` 
    
10. Inside the JavaScript file, add the above class to the new thumbnail:
    
     `thumb.classList.add("thumb");` 
    
11. Add the new thumbnail to the HTML element that contains the list of thumbnails:
    
     `imageThumbs.appendChild(thumb);` 
    

googletag.cmd.push(function() { googletag.display('adsninja-ad-unit-characterCountRepeatable-636c2cc1cf2a8-REPEAT3'); });

## How to Change the Image When the User Clicks on a Thumbnail

When the user clicks on one of the thumbnails, change the enlarged image in the center of the page to the selected image. You can add this functionality inside the JavaScript file.

1. At the top of the JavaScript file, get the HTML element of the currently selected image:
    
     `var currentImage = document.getElementById("current-image");` 
    
2. Inside the for-loop, [add an event handler](https://www.makeuseof.com/javascript-event-listeners-how-to-use/) that triggers when the user selects any of the thumbnails at the bottom of the page:
    
     `thumb.addEventListener(  
      "click", function() {  
      
      }  
    );` 
    
3. Inside the event handler, change the "src" attribute of the current image to the newly selected image. You can also update the "alt" attribute:
    
     `currentImage.src = this.src;  
    currentImage.alt = this.alt;` 
    
4. Click on the "index.html" file to open it in a web browser.
    
      
    
5. Select any of the thumbnails to view the image.
    
      
    

## Continue Expanding Your JavaScript Knowledge

Regardless of your experience, it's important to keep building projects to expand your knowledge. Continue to explore other projects such as building a chess game, calculator, or to-do list.