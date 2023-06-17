<script>
    import { _ } from '../lib/services/localization/i18n'
    import LayoutSimple from './_LayoutSimple.svelte'
    import { onMount } from 'svelte'
    import {
        activateAccount
    } from '../lib/services/customer'
    import { routeParams } from '../lib/router'
    
    let showOk = false
    let showCriticalError = false

    onMount(async () => {
        let token= $routeParams.encodedToken
        
        const result = await activateAccount(token)
        console.log('result',result)
        if(result.status == 'ok') {
            showOk = true
        } else {
            showCriticalError = true
        }  
    })
</script>

<LayoutSimple>
    <div class="container-fluid">
        <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 row bg-light bg-kebab">
            <div class="col-12">
                <img
                    src="assets/img/logo.png"
                    class="mx-auto d-block img-fluid my-5"
                    alt="logo"
                />
            </div>
        </div>
        <div class="mx-auto  my-5 col-md-11 col-xxl-7">
            {#if showCriticalError}
                <div class="alert alert-danger" role="alert">
                    {$_('register.activationError')}
                </div>
            {/if}
            {#if showOk}
                <div class="alert alert-success" role="alert">
                    {$_('register.activationOk')}
                </div>
            {/if}
        </div>
    </div>
</LayoutSimple>
