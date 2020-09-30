const url = "https://localhost:5001/api/beanvariety/";

const button = document.querySelector("#run-button");
button.addEventListener("click", () => {
  getAllBeanVarieties()
    .then(beanVarieties => {
      console.log(beanVarieties);
    })
});

function getAllBeanVarieties() {
  return fetch(url).then(resp => resp.json());
}

// Adding new bean
document.getElementById("add-bean-button").addEventListener("click", async () => {
  const newBean = {
    Name: document.getElementById("bean-name").value,
    Region: document.getElementById("bean-region").value,
    Notes: document.getElementById("bean-notes").value
  }

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(newBean)
  })
  const result = await res.json();
  return result.then(() => {
    document.getElementById("new-bean-form").reset();
  });

})