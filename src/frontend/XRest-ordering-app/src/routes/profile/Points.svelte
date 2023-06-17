<script>
    import LayoutSimple from '../_LayoutSimple.svelte'
    import ProgressBar  from '../../lib/components/ProgressBar.svelte'
    import { _ } from '../../lib/services/localization/i18n'
    import { onMount } from 'svelte'
    import { myProfileGetPointsHistory } from '../../lib/services/customer'
    import { authData } from '../../lib/services/customer'
    import { push } from '../../lib/router'

    if(!$authData.isLogged) {
        push('/')
    }

    onMount(() => {
        loading = true
        setTimeout(() => {
            myProfileGetPointsHistory()
            .then(result => {
                loading = false
                total = result.total
                items = result.items
            })
        }, 300);
    })

    let loading = true
    let total = 0
    let items = []
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
                <h4 class="card-title">{$_("userProfile.points.title")}</h4>
                <div class="table-responsive">
                    <table class="table">
                        <thead>
                            <tr>
                              <th scope="col">{$_("userProfile.points.dateColumn")}</th>
                              <th scope="col">{$_("userProfile.points.commentColumn")}</th>
                              <th scope="col">{$_("userProfile.points.pointsColumn")}</th>
                            </tr>
                        </thead>
                        <tbody>
                        {#if loading}
                            <tr>
                                <td colspan="3">
                                    <ProgressBar/>
                                </td>
                            </tr>
                        {:else}
                            {#if items.length === 0}
                                <tr>
                                    <td colspan="3" class="text-center">
                                        {$_("userProfile.points.empty")}
                                    </td>
                                </tr>
                            {:else}
                                {#each  items as item }
                                    <tr>
                                        <th scope="row">{item.created}</th>
                                        <td>{item.comment}</td>
                                        <td>{item.points}</td>
                                    </tr>
                                {/each}
                                <tr>
                                    <th ></th>
                                    <td class="text-end " ><strong>{$_("userProfile.points.total")}</strong></td>
                                    <td>{total}</td>
                                </tr>
                            {/if}
                        {/if}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</LayoutSimple>
