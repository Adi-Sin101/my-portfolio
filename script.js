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
