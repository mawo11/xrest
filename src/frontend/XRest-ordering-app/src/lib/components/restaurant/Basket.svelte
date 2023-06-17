<script>
  import ProgressBar from '../ProgressBar.svelte'
  import { onMount, onDestroy } from 'svelte'
  import { selectedRestaurant } from '../../stores'
  import { _, locale } from '../../services/localization/i18n'
  import { authData } from '../../services/customer'
  import { configuredProduct } from './ConfiguratorDialog.svelte'
  import {
    setCallbackForRefresh,
    basketGetView,
    basketRemoveItem,
    basketIncraseItem,
    basketDecraseItem,
    basketGetItemDetails,
    basketUpdateItemNote,
  } from '../../services/basketApi'

  import { push } from '../../router'

  let loadingData = true
  let basketView = { items: [] }
  let editingBasketItem = null
  export let onSummary = false

  onMount(async () => {
    setCallbackForRefresh(refreshBasket)

    setTimeout(() => {
      refreshBasket()
    }, 500)
  });

  let unsubscribeLocale = locale.subscribe( (val) =>{
    refreshBasket()
	})

  onDestroy(() => {
		try {
			unsubscribeLocale()
		} catch (e) {}
	})

  async function refreshBasket() {
    loadingData = true
    basketView = (await basketGetView()) || { items: [] }
    loadingData = false
  }

  async function incItem(basketItem) {
    basketView = await basketIncraseItem(basketItem.id)
  }

  async function decItem(basketItem) {
    basketView = await basketDecraseItem(basketItem.id)
  }

  async function configure(basketItem) {  
    let product =  await basketGetItemDetails(basketItem.id)
    if (product.notFound) {
      await refreshBasket()
    }
    configuredProduct.set(product)
  }

  async function remove(basketItem) {
    basketView = await basketRemoveItem(basketItem.id)
  }

  let editNoteMode = false
  async function editNote(basketItem) {
    basketItem.editNoteMode = true
    editNoteMode = true
    editingBasketItem = basketItem
    basketView = basketView
  }

  async function goSummary() {

    push('/summary')
  }

  async function updateNote() {
    if (!editNoteMode) {
      return 
    }
    editNoteMode = false
    basketView = await basketUpdateItemNote(editingBasketItem.id, editingBasketItem.note)
  }

  async function  onKeyPress(e) {
    if (!editNoteMode) {
      return 
    }
  
    if (e.charCode === 13) {
      editNoteMode = false
      basketView = await basketUpdateItemNote(editingBasketItem.id, editingBasketItem.note)
    }
  }
</script>

{#if $selectedRestaurant.working}
  <div class="card w-100 mx-auto mt-2 ">
    <div class="card-header bg-dark text-light text-end">
      <span class="mx-3 d-inline d-md-none">{$_("basket.title")}</span>
      <a data-bs-toggle="collapse"
        role="button"
        aria-controls="body-basket"
        href="#body-basket"
        class="text-decoration-none link-light"
        aria-expanded="true"><i class="fas fa-window-minimize" /></a>
    </div>
    <div class="card-body collapse show" id="body-basket">
      <h5 class="card-title text-center">{$_("basket.title")}</h5>
      <hr />
      {#if loadingData}
        <ProgressBar />
      {:else if basketView.items!=null && basketView.items.length > 0}
        {#each basketView.items as basketItem}
          <div class="row">
            <div class="text-start fw-bold w-75 d-inline">
             {basketItem.title}
            </div>
            <div class="text-end fw-bold w-25 d-inline text-nowrap">{basketItem.price}</div>
            {#if basketItem.subProducts != null && basketItem.subProducts.length > 0 }
              {#each basketItem.subProducts as  subProduct}
                <div class="mx-1 fw-lighter fst-italic">{subProduct}</div>
              {/each}
            {/if}
            {#if basketItem.bundles != null && basketItem.bundles.length > 0 }
              {#each basketItem.bundles as bundleItem}
                <div class="mx-1 fw-lighter fst-italic">{bundleItem.title}</div>
                {#if bundleItem.subProducts != null && bundleItem.subProducts.length > 0 }
                  {#each bundleItem.subProducts as  subProduct}
                    <div class="mx-3 fw-lighter fst-italic">{subProduct}</div>
                  {/each}
                {/if}
              {/each}
            {/if}
            {#if (basketItem.note != null && basketItem.note != '') || (basketItem.editNoteMode != undefined && basketItem.editNoteMode )}
              <div class="input-group input-group-sm mb-3  d-flex align-items-center">
                <div class="input-group-prepend">
                  <span class="input-group-text background-yellow text-dark">{$_("basket.note")}</span>
                </div>
                {#if (basketItem.editNoteMode != undefined && basketItem.editNoteMode ) }
                  <input
                      type="text"
                      name="info"
                      autocomplete="off"
                      maxlength="250"
                      on:blur={updateNote}
                      on:keypress={onKeyPress}
                      bind:value={basketItem.note}
                      class="form-control" />
                {:else}
                    <div class="note-info">{basketItem.note}</div>
                {/if}
              </div>
            {/if}
            {#if !basketItem.readOnly &&  !onSummary}
              <div class="product_basket_buttons">
                <button class="btn btn-sm btn-danger btn-basket" on:click={() => decItem(basketItem)}><i class="fa fa-minus" /></button>
                <div class="basket-qty"> {basketItem.count} </div>
                <button class="btn btn-sm btn-success btn-basket" on:click={() => incItem(basketItem)}><i class="fa fa-plus" /></button>
                <button class="btn btn-sm btn-info btn-basket" on:click={() => configure(basketItem)}><i class="fa fa-list" /></button>
                <button class="btn btn-sm btn-warning btn-basket" on:click={() => editNote(basketItem)}><i class="fas fa-edit" /></button>
                <button class="btn btn-sm btn-light btn-basket" on:click={() => remove(basketItem)}><i class="fas fa-trash-alt" /></button>
              </div>
              {/if}
            </div>         
          {/each}
        <div class="clearfix" />
          <div class="product_basket_summary_container">
            {#if basketView.isDelivey }
              <div class="product_basket_title">{$_("basket.deliveryPrice")}</div>
              <div class="product_basket_title product_basket_price">
                {#if basketView.transport != null }
                  {basketView.transport}
                {:else}
                  {$_("basket.itsFree")}
                {/if}
              </div>
            {/if}
            <div class="product_basket_title">{$_("basket.totalOrder")}</div>
            <div class="product_basket_title product_basket_price">
              {basketView.total}
            </div>
          </div>
          <div class="clearfix" />
        {#if $selectedRestaurant.minOrder != null && basketView.isDelivey && !basketView.canSubmit && !onSummary}
          <div class="alert alert-danger text-center" role="alert">
            {$_("basket.minOrderTotalRequired")} <b>{$selectedRestaurant.minOrder}</b>
          </div>
        {/if}
        {#if basketView.canSubmit && !onSummary }
          <button type="button" class="btn btn-success w-100" on:click={goSummary}>{$_("basket.summary")}</button>
        {/if}
      {:else}
        <div class="g-3 mb-2 mt-2">{$_("basket.noItems")}</div>
      {/if}
    </div>
  </div>
{/if}
<style>
 .basket-qty {
   display: inline-block;
   width: 50px;
   height: 50px;
   text-align: center;
 }
 .note-info {
    padding-left: 5px;
    width: 70%;
 }
</style>
