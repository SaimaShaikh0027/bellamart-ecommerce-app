// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const searchInput = document.querySelector('#product-search');
const categoryTabs = document.querySelectorAll('.category-tab');
const productCards = document.querySelectorAll('.product-card');
const emptyState = document.querySelector('#empty-state');
const cartCount = document.querySelector('#cart-count');
let selectedCategory = 'All';
let itemsInCart = 0;

function filterProducts() {
	const searchTerm = (searchInput?.value || '').toLowerCase().trim();
	let visibleProducts = 0;

	productCards.forEach(card => {
		const matchesCategory = selectedCategory === 'All' || card.dataset.category === selectedCategory;
		const matchesSearch = !searchTerm || card.dataset.name.includes(searchTerm);
		const isVisible = matchesCategory && matchesSearch;
		card.hidden = !isVisible;
		if (isVisible) visibleProducts++;
	});

	if (emptyState) emptyState.hidden = visibleProducts > 0;
}

searchInput?.addEventListener('input', filterProducts);
categoryTabs.forEach(tab => tab.addEventListener('click', () => {
	selectedCategory = tab.dataset.category;
	categoryTabs.forEach(categoryTab => categoryTab.classList.toggle('active', categoryTab === tab));
	filterProducts();
}));
document.querySelectorAll('.add-button').forEach(button => button.addEventListener('click', () => {
	itemsInCart++;
	if (cartCount) cartCount.textContent = itemsInCart;
}));

const quantityInput = document.querySelector('#quantity');
document.querySelectorAll('[data-quantity-action]').forEach(button => button.addEventListener('click', () => {
	const change = button.dataset.quantityAction === 'increase' ? 1 : -1;
	quantityInput.value = Math.max(1, Math.min(99, Number(quantityInput.value) + change));
}));
document.querySelector('.detail-cart')?.addEventListener('click', () => {
	itemsInCart += Number(quantityInput?.value || 1);
	if (cartCount) cartCount.textContent = itemsInCart;
});
quantityInput?.addEventListener('input', () => {
	const quantity = Math.max(1, Math.min(99, Number(quantityInput.value) || 1));
	quantityInput.value = quantity;
	document.querySelectorAll('#detail-quantity, #buy-now-quantity').forEach(input => input.value = quantity);
});
