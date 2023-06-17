<script>
    import LayoutSimple from './_LayoutSimple.svelte'
    import ProgressBar from '../lib/components/ProgressBar.svelte'
    import { _ } from '../lib/services/localization/i18n'
    import { getRestaurantTerms } from '../lib/services/api'
    import { routeParams, push } from '../lib/router'

    let restaurantId = $routeParams.restaurantId
    
    if (restaurantId=== undefined || restaurantId == 0) {
        push('/')
    }

    let termsResponse = getRestaurantTerms(restaurantId);
</script>

<LayoutSimple>
    <div class="container-fluid">
        <div
            class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo"
                />
            </div>
        </div>
        <div class="mx-auto card my-5 col-md-11 col-xxl-7">
            <div class="card-header bg-white">
                <ul class="nav card-header-pills justify-content-center fs-4 search-address">
                    <li class="nav-item text-uppercase">{$_("terms.title")}</li>
                </ul>
            </div>
            <div class="card-body">
                {#await termsResponse}
                    <ProgressBar />
                {:then termsResponse}
                    {#if termsResponse.status == "ok"}
                        {@html termsResponse.text}
                    {:else}
                        {$_("terms.error")}
                    {/if}
                {/await}
            </div>
        </div>
    </div>
</LayoutSimple>
