// Contact form handler: show thank you and clear
document.addEventListener('DOMContentLoaded', function () {
    var contactForm = document.getElementById('contactForm');
    if (contactForm) {
        contactForm.addEventListener('submit', function (e) {
            e.preventDefault();
            alert('Thank you for contacting me! I will get back to you soon.');
            contactForm.reset();
        });
    }

    // Hamburger Menu Functionality
    const hamburger = document.getElementById('hamburger');
    const navLinks = document.getElementById('navLinks');
    
    if (hamburger && navLinks) {
        hamburger.addEventListener('click', function() {
            hamburger.classList.toggle('active');
            navLinks.classList.toggle('active');
        });

        // Close menu when clicking on a nav link (mobile)
        const navLinksItems = navLinks.querySelectorAll('a');
        navLinksItems.forEach(link => {
            link.addEventListener('click', function() {
                if (window.innerWidth <= 768) {
                    hamburger.classList.remove('active');
                    navLinks.classList.remove('active');
                }
            });
        });

        // Close menu when clicking outside (mobile)
        document.addEventListener('click', function(e) {
            if (window.innerWidth <= 768) {
                if (!hamburger.contains(e.target) && !navLinks.contains(e.target)) {
                    hamburger.classList.remove('active');
                    navLinks.classList.remove('active');
                }
            }
        });

        // Handle window resize
        window.addEventListener('resize', function() {
            if (window.innerWidth > 768) {
                hamburger.classList.remove('active');
                navLinks.classList.remove('active');
            }
        });
    }

    // Navigation active state functionality
    const navLinksAll = document.querySelectorAll('.nav-links a');
    const sections = document.querySelectorAll('section, header[id]');

    // Function to set active nav link
    function setActiveNavLink(activeLink) {
        navLinksAll.forEach(link => link.classList.remove('active'));
        activeLink.classList.add('active');
    }

    // Handle nav link clicks
    navLinksAll.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href').substring(1);
            const targetSection = document.getElementById(targetId);

            if (targetSection) {
                // Smooth scroll to section
                targetSection.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });

                // Set active state
                setActiveNavLink(this);
            }
        });
    });

    // Update active nav link based on scroll position
    function updateActiveNavOnScroll() {
        let current = '';

        sections.forEach(section => {
            const sectionTop = section.offsetTop;
            const sectionHeight = section.clientHeight;
            if (window.pageYOffset >= sectionTop - 100) {
                current = section.getAttribute('id');
            }
        });

        navLinksAll.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('href') === `#${current}`) {
                link.classList.add('active');
            }
        });
    }

    // Add scroll event listener
    window.addEventListener('scroll', updateActiveNavOnScroll);

    // Set initial active state
    updateActiveNavOnScroll();
});

// Updated role rotation without Tech-Business Case Competitor
const roles = [
    "Web Developer",
    "App Developer", 
    "Programmer",
    "CS Student"
];

let index = 0;
const roleElement = document.getElementById("role-text");

function rotaterole() {
    if (roleElement) {
        roleElement.classList.add("fade-out");
        
        setTimeout(() => {
            roleElement.textContent = roles[index];
            roleElement.classList.remove("fade-out");
            index = (index + 1) % roles.length;
        }, 500);
    }
}

// Start role rotation after page loads
document.addEventListener('DOMContentLoaded', function() {
    setInterval(rotaterole, 2500);
});

// Fade-in scroll animation
const faders = document.querySelectorAll(".fade-in");

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add("visible");
        }
    });
}, { threshold: 0.1 });

faders.forEach(fadeEl => observer.observe(fadeEl));

// Modal logic
const modal = document.getElementById("experienceModal");
const modalImage = document.getElementById("modalImage");
const modalText = document.getElementById("modalText");
const closeModal = document.querySelector(".close-modal");

if (modal && modalImage && modalText && closeModal) {
    document.querySelectorAll(".experience-card").forEach(card => {
        card.addEventListener("click", () => {
            const imgSrc = card.querySelector("img").src;
            const title = card.querySelector("h4").textContent;
            const company = card.querySelector(".company").textContent;
            const desc = card.querySelector("p:not(.company)").textContent;

            modalImage.src = imgSrc;
            modalText.innerHTML = `<h3>${title}</h3><p><strong>${company}</strong></p><p>${desc}</p>`;
            modal.style.display = "block";
        });
    });

    closeModal.addEventListener("click", () => {
        modal.style.display = "none";
    });

    window.addEventListener("click", e => {
        if (e.target === modal) modal.style.display = "none";
    });
}


