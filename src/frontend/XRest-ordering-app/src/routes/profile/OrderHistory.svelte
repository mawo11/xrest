<script>
    import LayoutSimple from '../_LayoutSimple.svelte'
    import ProgressBar from '../../lib/components/ProgressBar.svelte' 
    import { _ } from '../../lib/services/localization/i18n'
    import { authData } from '../../lib/services/customer'
    import { myProfileGetMyOrders } from '../../lib/services/customer'
    import { push } from '../../lib/router'

    if(!$authData.isLogged) {
        push('/')
    }

    let historyResponse = myProfileGetMyOrders();
</script>

<LayoutSimple>
    <div class="container-fluid">
        <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo" />
            </div>
        </div>
        
        <div class="mx-auto card my-5 col-md-11 col-xxl-8 col-12">
            <div class="card-body">
                <h4 class="card-title">{$_("userProfile.orderHistory")}</h4>
                {#await historyResponse}
                    <ProgressBar />
                {:then historyResponse}
                    {#if historyResponse.status == "ok"}
                        <div class="table-responsive">
                            <table class="table">
                                <thead>
                                    <tr>
                                    <th scope="col">{$_("userProfile.myorders.orderId")}</th>
                                    <th scope="col">{$_("userProfile.myorders.restaurantName")}</th>
                                    <th scope="col">{$_("userProfile.myorders.date")}</th>
                                    <th scope="col">{$_("userProfile.myorders.status")}</th>
                                    <th scope="col">{$_("userProfile.myorders.amount")}</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {#if historyResponse.orders.length === 0}
                                        <tr>
                                            <td colspan="3" class="text-center">
                                                {@html $_("userProfile.myorders.empty")}
                                            </td>
                                        </tr>
                                    {:else}
                                        {#each  historyResponse.orders as item }
                                            <tr>
                                                <th scope="row">{item.id}</th>
                                                <td>{item.restaurantName}</td>
                                                <td>{item.createdDate}</td>
                                                <td>{item.status}</td>
                                                <td>{item.amount}</td>
                                            </tr>
                                        {/each}
                                    {/if}
                                </tbody>
                            </table>
                        </div>
                    {:else}
                        {@html $_("userProfile.myorders.error")}
                    {/if}
                {/await}     
            </div>
        </div>
    </div>
</LayoutSimple>
