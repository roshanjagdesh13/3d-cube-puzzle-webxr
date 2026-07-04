# 🚀 Deployment Guide

Complete guide for deploying the 3D Color Cube Puzzle to various platforms.

---

## Deployment Options

1. [GitHub Pages](#github-pages) - Free web hosting
2. [Netlify](#netlify) - Modern web platform
3. [Vercel](#vercel) - Optimized for web apps
4. [itch.io](#itchio) - Game-focused hosting
5. [Custom Server](#custom-server) - Full control

---

## GitHub Pages

**Best for:** Open-source projects, portfolios  
**Cost:** Free  
**Setup Time:** 5 minutes

### Method 1: Direct Push

```bash
# 1. Initialize git repository
cd "c:\Users\Roshan\Downloads\Unity-Project (2)\Unity-Project"
git init

# 2. Add files
git add index.html README.md LICENSE .gitignore
git add ARCHITECTURE.md VR_AR_GUIDE.md DEPLOYMENT.md

# 3. Commit
git commit -m "Initial commit: 3D Cube Puzzle"

# 4. Add remote (replace YOUR_USERNAME)
git remote add origin https://github.com/YOUR_USERNAME/3d-cube-puzzle.git

# 5. Push to main
git branch -M main
git push -u origin main

# 6. Enable GitHub Pages
# Go to: Settings → Pages → Source: main branch → Save
```

### Method 2: GitHub Actions (Automated)

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy to GitHub Pages

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      
      - name: Deploy to GitHub Pages
        uses: JamesIves/github-pages-deploy-action@4.1.5
        with:
          branch: gh-pages
          folder: .
```

**Your site will be live at:**  
`https://YOUR_USERNAME.github.io/3d-cube-puzzle/`

---

## Netlify

**Best for:** Professional projects with custom domain  
**Cost:** Free (with paid options)  
**Setup Time:** 2 minutes

### Via Drag-and-Drop

1. Go to [Netlify](https://www.netlify.com/)
2. Sign up / Log in
3. Drag your project folder onto the page
4. Done! Site is live instantly

### Via GitHub (Continuous Deployment)

1. Push code to GitHub
2. Log in to Netlify
3. **New site from Git** → Select repo
4. Build settings:
   - Build command: (leave empty)
   - Publish directory: `/`
5. Click **Deploy site**

**Your site URL:**  
`https://random-name-12345.netlify.app`

**Add Custom Domain:**
```
Site Settings → Domain Management → Add custom domain
```

---

## Vercel

**Best for:** Web applications, serverless functions  
**Cost:** Free (with paid options)  
**Setup Time:** 2 minutes

### Via GitHub

1. Push code to GitHub
2. Go to [Vercel](https://vercel.com/)
3. Import project from GitHub
4. Deploy (zero configuration needed)

### Via CLI

```bash
# Install Vercel CLI
npm install -g vercel

# Deploy
cd "c:\Users\Roshan\Downloads\Unity-Project (2)\Unity-Project"
vercel

# Follow prompts
# Your site is live!
```

**Your site URL:**  
`https://3d-cube-puzzle.vercel.app`

---

## itch.io

**Best for:** Indie games, game showcases  
**Cost:** Free  
**Setup Time:** 5 minutes

### Steps

1. Go to [itch.io](https://itch.io/)
2. Create account
3. **Dashboard → Create new project**
4. Fill in details:
   - Title: 3D Color Cube Puzzle
   - Kind: HTML
   - Classification: Game
5. Upload ZIP file:
   ```bash
   # Create ZIP of your project
   # Include: index.html, README.md, LICENSE
   ```
6. **Set as playable in browser:**
   - ✓ This file will be played in the browser
   - Viewport dimensions: 1200 x 800
   - Frame options: Fullscreen button
7. Publish!

**Your game URL:**  
`https://yourusername.itch.io/3d-cube-puzzle`

---

## Custom Server

**Best for:** Full control, advanced features  
**Cost:** $5-20/month  
**Setup Time:** 30 minutes

### Option 1: Simple HTTP Server

#### Python
```bash
cd "c:\Users\Roshan\Downloads\Unity-Project (2)\Unity-Project"
python -m http.server 8000
```

#### Node.js
```bash
npx http-server -p 8000
```

### Option 2: Nginx (Production)

#### Install Nginx
```bash
# Ubuntu/Debian
sudo apt update
sudo apt install nginx

# Start service
sudo systemctl start nginx
```

#### Configure
```nginx
# /etc/nginx/sites-available/cube-puzzle
server {
    listen 80;
    server_name yourdomain.com;
    
    root /var/www/cube-puzzle;
    index index.html;
    
    location / {
        try_files $uri $uri/ =404;
    }
    
    # Enable gzip compression
    gzip on;
    gzip_types text/html text/css application/javascript;
}
```

#### Deploy Files
```bash
# Copy files to server
scp -r * user@yourserver:/var/www/cube-puzzle/

# Enable site
sudo ln -s /etc/nginx/sites-available/cube-puzzle /etc/nginx/sites-enabled/
sudo systemctl reload nginx
```

### Option 3: Docker Container

Create `Dockerfile`:

```dockerfile
FROM nginx:alpine

COPY index.html /usr/share/nginx/html/
COPY *.md /usr/share/nginx/html/

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
```

Build and run:

```bash
docker build -t cube-puzzle .
docker run -d -p 80:80 cube-puzzle
```

---

## SSL/HTTPS Setup

### Free SSL with Let's Encrypt

```bash
# Install Certbot
sudo apt install certbot python3-certbot-nginx

# Get certificate
sudo certbot --nginx -d yourdomain.com

# Auto-renewal is set up automatically
```

---

## Custom Domain Setup

### GitHub Pages

1. Buy domain (Namecheap, Google Domains, etc.)
2. Add DNS records:
   ```
   Type: CNAME
   Name: www
   Value: yourusername.github.io
   ```
3. In GitHub repo:
   - Settings → Pages → Custom domain
   - Enter: www.yourdomain.com
   - Save

### Netlify/Vercel

1. Site settings → Domain management
2. Add custom domain
3. Follow DNS instructions
4. Wait for DNS propagation (24-48 hours)

---

## Performance Optimization

### 1. Enable Compression

Add to `.htaccess` (Apache):
```apache
<IfModule mod_deflate.c>
    AddOutputFilterByType DEFLATE text/html
    AddOutputFilterByType DEFLATE text/css
    AddOutputFilterByType DEFLATE application/javascript
</IfModule>
```

### 2. Add Caching Headers

```apache
<IfModule mod_expires.c>
    ExpiresActive On
    ExpiresByType text/html "access plus 1 hour"
    ExpiresByType text/css "access plus 1 month"
    ExpiresByType application/javascript "access plus 1 month"
</IfModule>
```

### 3. Minify HTML (Optional)

```bash
# Install html-minifier
npm install -g html-minifier

# Minify
html-minifier index.html -o index.min.html --collapse-whitespace --remove-comments

# Use index.min.html in production
```

---

## Analytics Setup

### Google Analytics

Add before `</head>`:

```html
<!-- Google Analytics -->
<script async src="https://www.googletagmanager.com/gtag/js?id=GA_MEASUREMENT_ID"></script>
<script>
  window.dataLayer = window.dataLayer || [];
  function gtag(){dataLayer.push(arguments);}
  gtag('js', new Date());
  gtag('config', 'GA_MEASUREMENT_ID');
</script>
```

### Simple Analytics (Privacy-friendly)

```html
<script async defer src="https://scripts.simpleanalyticscdn.com/latest.js"></script>
<noscript><img src="https://queue.simpleanalyticscdn.com/noscript.gif" alt="" referrerpolicy="no-referrer-when-downgrade" /></noscript>
```

---

## SEO Optimization

### Add Meta Tags

```html
<head>
    <!-- Basic Meta -->
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>3D Color Cube Puzzle - Interactive VR/AR Ready Game</title>
    <meta name="description" content="Play the interactive 3D Color Cube Puzzle game. VR and AR ready architecture with 8 colors, 150 cells, and full 3D rotation.">
    <meta name="keywords" content="3D puzzle, cube game, VR game, AR game, interactive puzzle, color cube">
    <meta name="author" content="Roshan">
    
    <!-- Open Graph (Facebook, LinkedIn) -->
    <meta property="og:title" content="3D Color Cube Puzzle">
    <meta property="og:description" content="Interactive 3D puzzle game with VR/AR ready architecture">
    <meta property="og:image" content="https://yourdomain.com/screenshot.png">
    <meta property="og:url" content="https://yourdomain.com">
    <meta property="og:type" content="website">
    
    <!-- Twitter Card -->
    <meta name="twitter:card" content="summary_large_image">
    <meta name="twitter:title" content="3D Color Cube Puzzle">
    <meta name="twitter:description" content="Interactive 3D puzzle game with VR/AR ready architecture">
    <meta name="twitter:image" content="https://yourdomain.com/screenshot.png">
    
    <!-- Favicon -->
    <link rel="icon" type="image/png" href="favicon.png">
</head>
```

### Create `robots.txt`

```
User-agent: *
Allow: /

Sitemap: https://yourdomain.com/sitemap.xml
```

### Create `sitemap.xml`

```xml
<?xml version="1.0" encoding="UTF-8"?>
<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
  <url>
    <loc>https://yourdomain.com/</loc>
    <lastmod>2026-07-04</lastmod>
    <priority>1.0</priority>
  </url>
</urlset>
```

---

## Monitoring

### Uptime Monitoring

**UptimeRobot** (Free):
1. Go to [UptimeRobot](https://uptimerobot.com/)
2. Add monitor → HTTPS
3. Enter your URL
4. Get alerts if site goes down

### Error Tracking

**Sentry** (Free tier):

```html
<script src="https://browser.sentry-cdn.com/7.0.0/bundle.min.js"></script>
<script>
  Sentry.init({
    dsn: "YOUR_SENTRY_DSN",
    environment: "production"
  });
</script>
```

---

## Backup Strategy

### Automated Git Backups

```bash
# Create backup script
#!/bin/bash
cd /path/to/project
git add .
git commit -m "Auto backup $(date)"
git push origin main
```

Save as `backup.sh` and schedule with cron:

```bash
# Run daily at 2 AM
0 2 * * * /path/to/backup.sh
```

---

## Troubleshooting

### Issue: 404 Not Found

**Solution:** Check file paths are correct
```bash
# Verify index.html exists
ls -la index.html
```

### Issue: CORS Errors

**Solution:** Add CORS headers (server config)
```nginx
add_header Access-Control-Allow-Origin *;
```

### Issue: Slow Loading

**Solution:**
1. Enable compression (gzip)
2. Add caching headers
3. Use CDN (Cloudflare free tier)

### Issue: Mobile Not Working

**Solution:** Check viewport meta tag:
```html
<meta name="viewport" content="width=device-width, initial-scale=1.0">
```

---

## Deployment Checklist

### Before Deployment

- [ ] Test in multiple browsers
- [ ] Test on mobile devices
- [ ] Check all links work
- [ ] Validate HTML
- [ ] Test performance (PageSpeed Insights)
- [ ] Add meta tags for SEO
- [ ] Set up favicon
- [ ] Write deployment README

### After Deployment

- [ ] Test live site thoroughly
- [ ] Set up SSL/HTTPS
- [ ] Configure custom domain (if any)
- [ ] Set up analytics
- [ ] Add to portfolio
- [ ] Share on social media
- [ ] Submit to web directories

---

## Quick Comparison

| Platform | Setup Time | Cost | Best For |
|----------|-----------|------|----------|
| **GitHub Pages** | 5 min | Free | Open source |
| **Netlify** | 2 min | Free | Professional |
| **Vercel** | 2 min | Free | Web apps |
| **itch.io** | 5 min | Free | Games |
| **Custom VPS** | 30 min | $5/mo | Full control |

---

## Recommended Approach

### For Portfolio/Resume

```
GitHub Pages + Custom Domain + Analytics
= Professional presence at minimal cost
```

### For Game Showcase

```
itch.io + GitHub (source code)
= Maximum visibility in gaming community
```

### For Production App

```
Netlify/Vercel + Custom Domain + CDN
= Best performance and reliability
```

---

## Next Steps

1. ✅ Choose deployment platform
2. ✅ Follow relevant section above
3. ✅ Deploy and test
4. ✅ Add to portfolio
5. ✅ Share with world!

---

**Need help?** Open an issue on GitHub!

---

**Last Updated:** 2026-07-04  
**Version:** 1.0.0  
**Author:** Roshan
