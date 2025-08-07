const roles=[
    "Web Developer",
    "App Developer",
    "Programmer",
    "Tech-Business Case Competitor", // 👆 Change anything inside this array to match your style.
// Each item will show up one by one every few seconds.
"CS Student"];

let index=0;
const roleElement=document.getElementById("role-text");
function rotaterole()
{
    roleElement.classList.add("fade-out");

   //js function
    setTimeout(() => {
        roleElement.textContent=roles[index];
        roleElement.classList.remove("fade-out");
        index=(index+1)%roles.length;
    }, 500); // Adjust the timeout to match the CSS transition duration.
}
setInterval(rotaterole,2500); // Change the interval to control how often the role changes.

document.addEventListener("DOMContentLoaded", () => {
  const observer = new IntersectionObserver(entries => {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        const fills = entry.target.querySelectorAll(".progress-fill");
        fills.forEach(fill => {
          fill.style.width = fill.getAttribute("data-percentage");
        });
      }
    });
  }, {
    threshold: 0.3
  });

  document.querySelectorAll(".skill-card").forEach(card => {
    observer.observe(card);
  });
});
// Dark mode toggle
document.getElementById("darkModeToggle").addEventListener("click", () => {
  document.body.classList.toggle("dark-mode");
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


