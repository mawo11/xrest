<script context="module">
    import { writable } from 'svelte/store';
    export const configuredProduct = writable(null);
</script>
<script>
    import { basketSavePoduct } from '../../services/basketApi'
    import { getImageUrl } from "../../services/api"
    import { _ } from '../../../lib/services/localization/i18n'
    import { onDestroy } from 'svelte'

    let currentProduct = null
    let myModal = null

   var subscription = configuredProduct.subscribe( (value) => {
        if (value == null) return
        currentProduct = value
        if (myModal === null) {
            myModal = new bootstrap.Modal(
                document.getElementById('configurator')
            )
        }

        myModal.show()
    });

    onDestroy(() => {
        subscription();
        try{
            if (myModal != null){
                myModal.dispose()
            }
        }catch(e){
        }
    })

	async function addToBasket() {
		let isError = false

        currentProduct.productSets.forEach( x=> {
            x.error = false
            if (x.required) { 
                let isSelected = false;
                x.items.forEach( y => {
                    if (y.selected) {
                        isSelected = true
                    }
                })

                if (!isSelected) {
                    x.error = true
                    isError = true
                } else {
                    x.error = false
                }
            }
        })

        if (isError) {
            currentProduct = currentProduct
            return
        }
        
        myModal.hide()
        await basketSavePoduct(currentProduct)
        currentProduct = null
        configuredProduct.set(null)
	}

	async function cancelConfigueProduct() {
        myModal.hide()
		
		currentProduct = null
        configuredProduct.set(null)
	}

    async function selectProductSetItem(selectedProductSetItemId, selectedProductSetItem) {
        if (currentProduct.productSets != null && currentProduct.productSets.length > 0) {
            currentProduct.productSets.forEach(productSet => {
                if (productSet.id != selectedProductSetItemId) {
                    return
                }

                productSet.items.forEach( productSetItem => {
                   productSetItem.selected = productSetItem.id == selectedProductSetItem.id
                })
            })

            currentProduct.productSets  = currentProduct.productSets
        }
    }

    async function selectBundleItem(bundleLabel, selectedBundleItem) {
        if (currentProduct.bundles != null && currentProduct.bundles.length > 0) {
            currentProduct.bundles.forEach( bundle => {
                if (bundle.label != bundleLabel) {
                    return
                }

                bundle.items.forEach( bundleItem => {
                     bundleItem.selected = bundleItem.productId == selectedBundleItem.productId
                })
            })
            
            currentProduct.bundles = currentProduct.bundles
        }
    }
</script>
<div class="modal" tabindex="-1" id="configurator">
	<div class="modal-dialog modal-dialog-centered modal-lg configure-modal modal-dialog-scrollable">
		<div class="modal-content">
			<div class="modal-header border-0  pb-0 ">
				<h5 class="modal-title configurator-title">{$_("menu.configuratorTitle")}</h5>
				<button type="button" class="btn-close" on:click|preventDefault|stopPropagation={cancelConfigueProduct} aria-label="Close" />
			</div>
			<div class="modal-body">
                {#if currentProduct != null}
                    <div class="text-center">
                        <img 
                            src={getImageUrl(currentProduct.imageUrl)}
                            width="200"
                            class="rounded mx-auto d-block"
                            alt={currentProduct.name}
                        />
                        <div class="fs-5 text-center mt-2 mb-3">{ currentProduct.title }</div>        
                    </div>
                    {#each currentProduct.productSets as productSet}
                        <div class="g-3 mb-2 mt-2">{productSet.title} 
                        {#if productSet.error}
                            <span class="show-productSet-error">{$_("menu.required")}</span>
                        {/if}

                        </div>
                        <div class="d-flex flex-wrap align-items-center {productSet.error ? "show-productSet-error": null}">
                            {#each productSet.items as productSetItem}
                                <button class=" btn  me-2 mb-2 text-nowrap {productSetItem.selected ? 'btn-success' : 'btn-light'}"
                                    on:click|preventDefault|stopPropagation={selectProductSetItem(productSet.id, productSetItem)}>{productSetItem.title}</button>
                            {/each}
                        </div>
                    {/each}
                    {#if currentProduct.bundles != null && currentProduct.bundles.length > 0}
                        {#each currentProduct.bundles as bundle}
                            <div class="g-3 mb-2 mt-2">{bundle.label}</div>
                            <div class="d-flex flex-wrap ">
                                {#each bundle.items as bundleItem}
                                    <button class=" btn {bundleItem.selected ? 'btn-success' : 'btn-light'} me-2 mb-2 text-nowrap "
                                        on:click|preventDefault|stopPropagation={selectBundleItem(bundle.label, bundleItem)}>{bundleItem.title}</button>
                                {/each}
                            </div>
                        {/each}
                    {/if}
                {/if}
			</div>
			<div class="modal-footer border-0">
				<button type="button" class="btn btn-danger" on:click|preventDefault|stopPropagation={cancelConfigueProduct}>{$_("menu.cancelButton")}</button>
				<button type="button" class="btn btn-success" on:click|preventDefault|stopPropagation={addToBasket}>
                    {#if currentProduct != null && currentProduct.id == null}
                        {$_("menu.addButton")}
                    {:else}
                        {$_("menu.updateButton")}
                    {/if}
                 
                </button>
			</div>
		</div>
	</div> 
</div>
<style>
	.configurator-title {
		text-align: center;
		border-bottom: 1px solid gray;
	}
	.modal-content {
		background-color: #ffffffdf !important;
	}
    .show-productSet-error { 
        color: red;
        font-weight: bold;
    }
</style>
