/// <reference path="jquery-1.3.2-vsdoc2.js"/>
var JDialog;

/* ******************* */
/* Constructor & Init  */
/* ******************* */
if (JDialog == undefined) {
    JDialog = function(settings) {
        // Member variables
        this.divFrame = null;               // Frame object
        this.divFrameContainerBody = null;  // Frame container object
        this.spanFrameTitle = '';           // Frame title
        this.scrollHandler = null;          // scroll handler
        this.divC0verFrame = null;          // c0ver frame
        this.divFrame = null;
        this.container = null;              // The container
        this.parentContainer = null;        // The parent container
        this.nextContainer = null;          // The next container
        this.loadingImage = null;           // The loading image
        this.isShowing = false;             // 
        this.initialized = false;           //
        this.scrollHandler = null;          // The scroll handler
        this.JDialogName = 'JDialog_' + JDialog.JDialogCount++;

        // Setup global control tracking
        JDialog.instances[this.JDialogName] = this;

        this.initJDialog(settings);

        // Check containerType
        if (JDialog.isUrl(this.settings['uri'])) {
            this.containerType = JDialog.ContainterType.Iframe;
        }
        else {
            this.containerType = JDialog.ContainterType.Div;
        }

        /*
        Create JDialog on fly
        */

        // frame container
        var jFrameContainer = document.createElement("div");
        jFrameContainer.className = "jFrameContainer";

        // frame container header
        var jFrameContainerHeader = document.createElement("div");
        jFrameContainerHeader.className = "jFrameContainerHeader";

        var jFrameContainerHeaderTitle = document.createElement("span");
        this.spanFrameTitle = jFrameContainerHeaderTitle;
        jFrameContainerHeaderTitle.id = 'jDialogTitle';
        jFrameContainerHeaderTitle.appendChild(document.createTextNode(this.settings.title));

        var jFrameContainerHeaderClose = document.createElement("div");
        jFrameContainerHeaderClose.className = "jFrameContainerHeaderClose";

        var jFrameContainerHeaderCloseButton = document.createElement("input");
        jFrameContainerHeaderCloseButton.type = "button";
        jFrameContainerHeaderCloseButton.style.visibility = this.settings.showCancelButton == true ? 'visible' : 'hidden';

        var me = this;
        jFrameContainerHeaderCloseButton.onclick = function() {
            me.hide();
            if (me.settings['cancelHandler'] != undefined && me.settings['cancelHandler'] != null)
                me.settings.cancelHandler();
        };

        jFrameContainerHeaderClose.appendChild(jFrameContainerHeaderCloseButton);
        jFrameContainerHeader.appendChild(jFrameContainerHeaderTitle);
        jFrameContainerHeader.appendChild(jFrameContainerHeaderClose);

        // frame container body
        this.divFrameContainerBody = document.createElement("div");
        this.divFrameContainerBody.className = "jFrameContainerBody";

        jFrameContainer.appendChild(jFrameContainerHeader);
        jFrameContainer.appendChild(this.divFrameContainerBody);

        if (this.settings['showDefaultButton'] == true) {
            // frame container footer
            var jFrameContainerFooter = document.createElement("div");
            jFrameContainerFooter.className = "jFrameContainerFooter";

            var jFrameContainerFooterDiv = document.createElement("div");
            var jFrameContainerFooterButton = document.createElement("input");
            jFrameContainerFooterButton.type = "button";
            jFrameContainerFooterButton.value = this.settings['defaultButtonTitle'];
            jFrameContainerFooterButton.onclick = function() {
                if (me.settings['defaultHandler'] != null)
                    me.settings.defaultHandler();
                me.hide();
            };

            jFrameContainerFooterDiv.appendChild(jFrameContainerFooterButton);
            jFrameContainerFooter.appendChild(jFrameContainerFooterDiv);

            jFrameContainer.appendChild(jFrameContainerFooter);
        }

        // Create c0verframe
        this.divC0verFrame = document.createElement("div");
        this.divC0verFrame.className = "DIVC0verFrame";

        this.divFrame = document.createElement("div");
        this.divFrame.className = "DIVjFrame";

        var jFrame = document.createElement("div");
        jFrame.className = "jFrame";

        jFrame.appendChild(jFrameContainer);
        this.divFrame.appendChild(jFrame);

        this.divC0verFrame.appendChild(this.divFrame);

        // setup coordinate
        this.divC0verFrame.style.top = '0px';
        this.divC0verFrame.style.left = '0px';
        this.divC0verFrame.style.width = JDialog.getDocumentWidth() + 'px';
        this.divC0verFrame.style.height = JDialog.getDocumentHeight() + 'px';

        // 
        this.isShowing = false;
    };
}

/********************* */
/* Static members
/********************* */
JDialog.instances = {}
JDialog.ContainterType = {
    Div: 0,
    Iframe: 1
};
JDialog.version = '2.0.1';
JDialog.JDialogCount = 0;
JDialog.zIndex = 9000;
JDialog.factor = 20;
JDialog.modalCount = 0;

JDialog.MIN_DIALOG = {
    WIDTH: 200,
    HEIGHT: 75
};
JDialog.MAX_DIALOG = {
    WIDTH: 850,
    HEIGHT: 600
};



/********************* */
/* Member functions
/********************* */
JDialog.prototype.initJDialog = function(settings) {
    this.customSettings = {};
    this.settings = settings;

    this.initSettings();        // Init JDialog's settings

    // Create JDialog
};

JDialog.prototype.initSettings = function() {
    this.ensureDefault = function(settingName, defaultValue) {
        this.settings[settingName] = (this.settings[settingName] == undefined) ? defaultValue : this.settings[settingName];
    };

    // The JDialog's default settings
    this.ensureDefault('uri', '');
    this.ensureDefault('title', JDialog.version);
    this.ensureDefault('defaultButtonTitle', 'OK');
    this.ensureDefault('showDefaultButton', true);
    this.ensureDefault('defaultHandler', null);
    this.ensureDefault('showCancelButton', true);
    this.ensureDefault('enableScrollHandler', false);
    //this.ensureDefault('mode', JDialog.JDialogMode.Div);
    this.ensureDefault('loadingImageUrl', '');
    this.ensureDefault('reload', true);
    this.ensureDefault('speed', 300);
    this.ensureDefault('step', 10);
    this.ensureDefault('customSettings', {});

    // No longer need
    delete this.ensureDefault;
};
// Public: (Deprecated) addSetting adds a setting value. If the value given is undefined or null then the default_value is used.
JDialog.prototype.addSetting = function(name, value, default_value) {
    if (value == undefined) {
        return (this.settings[name] = default_value);
    } else {
        return (this.settings[name] = value);
    }
};

// Public: (Deprecated) getSetting gets a setting. Returns an empty string if the setting was not found.
JDialog.prototype.getSetting = function(name) {
    if (this.settings[name] != undefined) {
        return this.settings[name];
    }

    return "";
};
JDialog.prototype.setTitle = function(title) {
    this.spanFrameTitle.innerHTML = title;
};
JDialog.prototype.setURI = function(uri) {
    this.settings['uri'] = uri;

    if (JDialog.isUrl(this.settings['uri'])) {
        this.containerType = JDialog.ContainterType.Iframe;
    }
    else {
        this.containerType = JDialog.ContainterType.Div;
    }
};
JDialog.prototype.setFrame = function(width, height) {
    if (width != undefined && height != undefined && !isNaN(width) && !isNaN(height)) {
        this.settings['customSettings']['frameWidth'] = width;
        this.settings['customSettings']['frameHeight'] = height;
    } else {
        this.settings['customSettings']['frameWidth'] = undefined;
        this.settings['customSettings']['frameHeight'] = undefined
    }
};
JDialog.prototype.doModal = function() {
    var me = this;

    if (me.isShowing == false) {
        if (me.containerType == JDialog.ContainterType.Iframe) {
            if (me.loadingImage == null) {
                me.loadingImage = document.createElement("img");
                me.loadingImage.src = me.settings['loadingImageUrl'];
            }

            me.loadingImage.className = 'loading';
            me.divFrameContainerBody.appendChild(me.loadingImage);

            //            if (me.settings['reload'] == true) {
            //                if (me.container != null) {
            //                    me.divFrameContainerBody.removeChild(me.container);
            //                    delete me.container;
            //                }
            //            }

            if (me.container == null) {
                // create iframe
                me.container = document.createElement("iframe");
                me.container.className = "fwindow";
                me.container.id = me.JDialogName;
                me.container.src = me.settings['uri'];

                JDialog.addEvent(this.container, 'load', function() {
                    // thiet lap che do hien thi cho container
                    me.container.style.display = 'block';
                    me.container.style.visibility = 'visible';

                    var fwidth = me.settings['customSettings']['frameWidth'] != undefined ? me.settings['customSettings']['frameWidth'] : $(JDialog.extractIFrameBody(me.container)).width();
                    var fheight = me.settings['customSettings']['frameHeight'] != undefined ? me.settings['customSettings']['frameHeight'] : $(JDialog.extractIFrameBody(me.container)).height();

                    fwidth = fwidth > JDialog.MAX_DIALOG.WIDTH ? JDialog.MAX_DIALOG.WIDTH : (fwidth < JDialog.MIN_DIALOG.WIDTH ? JDialog.MIN_DIALOG.WIDTH : fwidth);
                    fheight = fheight > JDialog.MAX_DIALOG.HEIGHT ? JDialog.MAX_DIALOG.HEIGHT : (fheight < JDialog.MIN_DIALOG.HEIGHT ? JDialog.MIN_DIALOG.HEIGHT : fheight);

                    // Neu la trinh duyet IE, thi + them space vao width va height
                    if (navigator.appName.indexOf("Microsoft") > -1) {
                        fwidth += 16;           // Horizontal space
                        fheight += 16;          // Vertical space
                    }
                    me.container.style.width = fwidth + 'px';
                    me.container.style.height = fheight + 'px';

                    // An loading image
                    me.loadingImage.className = 'loaded';

                    // Thiet lap width heigt cho containerBody
                    me.divFrame.style.top = (JDialog.getWindowHeight() / 2) - (me.divFrame.offsetHeight / 2) + pos.y + 'px';
                    me.divFrame.style.left = (JDialog.getWindowWidth() / 2) - (me.divFrame.offsetWidth / 2) + pos.x + 'px';
                    // Neu co' animated thi code o day
                });
            }

            if (document.getElementById('DIVPlaceHolder'))
                this.parentContainer = document.getElementById('DIVPlaceHolder');
            else {
                this.parentContainer = document.createElement("div");
                this.parentContainer.id = 'DIVPlaceHolder';
            }
        }
        else {
            if ((this.container = document.getElementById(this.settings['uri'])) == undefined) throw 'Khong ton tai container';

            if (this.container.nextSibling != null) this.nextContainer = this.container.nextSibling;
            this.parentContainer = this.container.parentNode;           // The container is last node

            var fwidth = me.settings['customSettings']['frameWidth'] != undefined ? me.settings['customSettings']['frameWidth'] : $(this.container).width();
            var fheight = me.settings['customSettings']['frameHeight'] != undefined ? me.settings['customSettings']['frameHeight'] : $(this.container).height();

            fwidth = fwidth > JDialog.MAX_DIALOG.WIDTH ? JDialog.MAX_DIALOG.WIDTH : (fwidth < JDialog.MIN_DIALOG.WIDTH ? JDialog.MIN_DIALOG.WIDTH : fwidth);
            fheight = fheight > JDialog.MAX_DIALOG.HEIGHT ? JDialog.MAX_DIALOG.HEIGHT : (fheight < JDialog.MIN_DIALOG.HEIGHT ? JDialog.MIN_DIALOG.HEIGHT : fheight);

            me.container.style.width = fwidth + 'px';
            me.container.style.height = fheight + 'px';
            me.container.style.overflow = 'auto';
        }

        me.container.style.display = 'block';
        me.container.style.visibility = 'visible';

        me.divFrameContainerBody.appendChild(me.container);

        this.divC0verFrame.style.display = this.divFrame.style.display = 'block';
        this.divC0verFrame.style.visibility = this.divFrame.style.visibility = 'visible';

        // overlay mode
        this.divC0verFrame.style.zIndex = JDialog.zIndex + JDialog.modalCount;

        document.getElementsByTagName("form")[0].appendChild(this.divC0verFrame);

        var factor = {};
        factor = JDialog.modalCount * JDialog.factor;

        var pos = JDialog.getScroll();
        me.divFrame.style.top = (JDialog.getWindowHeight() / 2) - (me.divFrame.offsetHeight / 2) + factor + pos.y + 'px';
        me.divFrame.style.left = (JDialog.getWindowWidth() / 2) - (me.divFrame.offsetWidth / 2) + factor + pos.x + 'px';

        if (me.settings['enableScrollHandler'] == true) {
            me.divFrame.style.position = 'absolute';

            me.scrollHandler = function() {
                var pos = JDialog.getScroll();
                setTimeout(function() {
                    me.divFrame.style.top = (JDialog.getWindowHeight() / 2) - (me.divFrame.offsetHeight / 2) + factor + pos.y + 'px';
                    me.divFrame.style.left = (JDialog.getWindowWidth() / 2) - (me.divFrame.offsetWidth / 2) + factor + pos.x + 'px';
                }, me.settings.speed);
            };

            JDialog.addEvent(window, 'scroll', me.scrollHandler);
        }

        this.isShowing = true;

        JDialog.modalCount++;
    } else {

    }
};

// Hide JDialog
JDialog.prototype.hide = function() {
    if (this.isShowing == true) {
        if (this.containerType == JDialog.ContainterType.Div) {
            this.container.style.display = 'none';
            this.container.style.visibility = 'hidden';

            if (this.nextContainer != null) this.parentContainer.insertBefore(this.container, this.nextContainer);
            else this.parentContainer.appendChild(this.container);

            this.container = null;
        } else if (this.containerType == JDialog.ContainterType.Iframe) {
            if (this.settings['reload'] == true) {
                if (this.container != null) {
                    this.divFrameContainerBody.removeChild(this.container);
                    delete this.container;
                }
            }
        }

        this.divC0verFrame.style.display = this.divFrame.style.display = 'none';
        this.divC0verFrame.style.visibility = this.divFrame.style.visibility = 'hidden';

        if (this.settings['enableScrollHandler'] == true) {
            // unregister scroll event
            JDialog.removeEvent(window, "scroll", this.scrollHandler);
        }

        this.isShowing = false
        if (JDialog.modalCount > 0) JDialog.modalCount--;
    }
};

// Release resource that holds by JDialog object
JDialog.prototype.dispose = function() {
    if (this.nextContainer != null) this.parentContainer.insertBefore(this.container, this.nextContainer);
    else this.parentContainer.appendChild(this.container);

    this.divC0verFrame.removeChild(this.divFrame);
    document.getElementsByTagName("form")[0].removeChild(this.divC0verFrame);
};

/********************* */
/* Static member functions
/********************* */
JDialog.addEvent = function(obj, evType, fn) {
    if (obj.addEventListener) {
        obj.addEventListener(evType, fn, false);
        return true;
    } else if (obj.attachEvent) {
        var r = obj.attachEvent("on" + evType, fn);
        return r;
    } else {
        return false;
    }
};

JDialog.removeEvent = function(obj, evType, fn) {
    if (obj.detachEvent) {
        obj.detachEvent('on' + evType, fn);
    } else
        obj.removeEventListener(evType, fn, false);
    fn = null;
};

JDialog.getDocumentWidth = function() {
    return document.body.clientWidth;
    //return document.body.clientWidth > 0 ? document.body.clientWidth : document.documentElement.clientWidth;
};

JDialog.getDocumentHeight = function() {
    //return document.body.parentNode.scrollHeight;       // new method for truely document height;
    return document.body.clientHeight > 0 ? document.body.clientHeight : document.documentElement.clientHeight
};

JDialog.getWindowWidth = function() {
    return window.innerWidth ? window.innerWidth : document.documentElement.clientWidth;
};

JDialog.getWindowHeight = function() {
    return window.innerHeight ? window.innerHeight : document.documentElement.clientHeight;
};

JDialog.getWindow = function() {
    return (document.compatMode != "BackCompat") ? document.documentElement : document.body;
};
JDialog.getScroll = function() {
    var scrOfX = 0, scrOfY = 0;
    if (typeof (window.pageYOffset) == 'number') {
        //Netscape compliant
        scrOfY = window.pageYOffset;
        scrOfX = window.pageXOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        //DOM compliant
        scrOfY = document.body.scrollTop;
        scrOfX = document.body.scrollLeft;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        //IE6 standards compliant mode
        scrOfY = document.documentElement.scrollTop;
        scrOfX = document.documentElement.scrollLeft;
    }

    return { 'x': scrOfX, 'y': scrOfY };
};
// Private: takes a URL, determines if it is relative and converts to an absolute URL
// using the current site. Only processes the URL if it can, otherwise returns the URL untouched
JDialog.completeURL = function(url) {
    if (typeof (url) !== "string" || url.match(/^https?:\/\//i) || url.match(/^\//)) {
        return url;
    }

    var currentURL = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ":" + window.location.port : "");

    var indexSlash = window.location.pathname.lastIndexOf("/");
    if (indexSlash <= 0) {
        path = "/";
    } else {
        path = window.location.pathname.substr(0, indexSlash) + "/";
    }

    return /*currentURL +*/path + url;
};

JDialog.isUrl = function(str) {
    var urlPattern = new RegExp("((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)", 'gi');
    return urlPattern.test(str);
};

JDialog.extractIFrameBody = function(iFrameEl) {
    var doc = null;
    if (iFrameEl.contentDocument) { // For NS6
        doc = iFrameEl.contentDocument;
    } else if (iFrameEl.contentWindow) { // For IE5.5 and IE6
        doc = iFrameEl.contentWindow.document;
    } else if (iFrameEl.document) { // For IE5
        doc = iFrameEl.document;
    } else {
        alert("Error: could not find sumiFrame document");
        return null;
    }
    return doc.body;
};