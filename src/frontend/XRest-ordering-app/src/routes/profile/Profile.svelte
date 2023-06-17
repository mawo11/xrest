<script>
    import LayoutSimple from '../_LayoutSimple.svelte'
    import WaitingButton from '../../lib/components/WatingButton.svelte'
    import { _ } from '../../lib/services/localization/i18n'
    import { pushNormalToast, isValidPassword, pushWarnToast } from '../../lib/helpers'
    import { onMount } from 'svelte'
    import { 
        authData,
        myProfileGetDetails,
        myProfileUpdateDetails,
        myProfileChangeMyPassword
    } from '../../lib/services/customer'
    import { push } from '../../lib/router'
   
    if(!$authData.isLogged) {
        push('/')
    }

    let showCriticalError = false

    let dataCtx = {
        firstname: null,
        lastname: null,
        phone: null,
        birthDay: "1",
        birthYear: "2021",
        birthMonth: "2"
    }

    let passCtx = { 
        pass1: null,
        pass2: null
    }

    onMount( async () => {
        const data = await myProfileGetDetails()

        if (data.success) {
            dataCtx.firstname = data.firstname
            dataCtx.lastname = data.lastname
            dataCtx.phone = data.phone
            const birthdate = new Date(data.birthdate)
            dataCtx.birthDay = birthdate.getDate()
            dataCtx.birthYear = birthdate.getFullYear()
            dataCtx.birthMonth = (birthdate.getMonth() + 1).toString()
        }
    })

    async function saveData() {
        const result = await myProfileUpdateDetails({
            phone: dataCtx.phone,
            firstname: dataCtx.firstname,
            lastname: dataCtx.lastname,
            Birthdate:  new Date(dataCtx.birthYear, dataCtx.birthMonth - 1, dataCtx.birthDay, 0, 0, 0, 0)
        })

        if (result.status == 'ok') {
            pushNormalToast($_("userProfile.yourData.saveDataOk"))
        }
    }

    async function changePassword() {
        if (isValidPassword(passCtx.pass1, passCtx.pass2)) {
            const result = await myProfileChangeMyPassword({
                password: passCtx.pass1
            })

            if (result.status === 'ok') {
                pushNormalToast($_("userProfile.yourData.changePasswordOK"))
            }
        } else {
            pushWarnToast($_("userProfile.yourData.passErr"))
        }
    }
    
    let saveDataProcessing = false
    function saveDataProcessingHandle(newValue) {
        saveDataProcessing = newValue
    }

    let changePasswordProcessing = false
    function changePasswordProcessingHandle(newValue) {
        changePasswordProcessing = newValue
    }
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
                <h4 class="card-title">{$_("userProfile.yourData.yoursAccountTitle")}</h4>
                <h6 class="card-subtitle mb-2 text-muted">{$_("userProfile.yourData.yoursDataTitle")}</h6>
                <div class="row">
                    <div class="my-3 col-4">
                        <input type="text" disabled={saveDataProcessing} bind:value={dataCtx.firstname} placeholder="{$_("userProfile.yourData.firstname")}" name="firstname" class="shadow form-control">
                    </div>
                    <div class="my-3 col-4">
                        <input type="text" disabled={saveDataProcessing} bind:value={dataCtx.lastname} placeholder="{$_("userProfile.yourData.lastname")}" name="lastname" class="shadow form-control">
                    </div>
                    <div class="my-3 col-4">
                        <input type="text" disabled={saveDataProcessing} bind:value={dataCtx.phone} placeholder="{$_("userProfile.yourData.phone")}" name="phone" class="shadow form-control">
                    </div>
                </div>
            </div>

            <div class="card-body">
                <h6 class="card-subtitle mb-2 text-muted">{$_("userProfile.yourData.birthData")}</h6>
                <div class="row">
                    <div class="my-3 col-4">
                        <input type="text" disabled={saveDataProcessing} bind:value={dataCtx.birthDay} placeholder="Dzień" name="dayborn" class="shadow form-control" maxlength="2" pattern="(?:30))|(?:(?:0[13578]|1[02])-31)">
                    </div>
                    <div class="my-3 col-4">
                        <select name="monthborn" disabled={saveDataProcessing} class="shadow form-select" bind:value={dataCtx.birthMonth} >
                            <option value="1">Styczeń</option>
                            <option value="2">Luty</option>
                            <option value="3">Marzec</option>
                            <option value="4">Kwiecień</option>
                            <option value="5">Maj</option>
                            <option value="6">Czerwiec</option>
                            <option value="7">Lipiec</option>
                            <option value="8">Sierpień</option>
                            <option value="9">Wrzesień</option>
                            <option value="10">Październik</option>
                            <option value="11">Listopad</option>
                            <option value="12">Grudzień</option>
                        </select>  
                    </div>
                    <div class="my-3 col-4">
                        <input type="text" disabled={saveDataProcessing} bind:value={dataCtx.birthYear} placeholder="Rok urodzenia" name="yearborn" class="shadow form-control" maxlength="4" pattern="[0-9]{4}" title="Podaj rok w formacie 4 cyfr">
                    </div>
                </div>
            </div>

            <div class="justify-content-end text-end pe-4">
                <WaitingButton callback={saveData} processingCallback={saveDataProcessingHandle}>
                    {$_('userProfile.save')}
                </WaitingButton>
            </div>

            <div class="card-body">
                <h4 class="card-title">{$_("userProfile.yourData.changePasswordTitle")}</h4>
                <h6 class="card-subtitle mb-2 text-muted">{$_("userProfile.yourData.emptyIfNotChange")}</h6>
                <div class="row">
                    <div class="my-3 col-4">
                        <input type="password" disabled={changePasswordProcessing} bind:value={passCtx.pass1} placeholder="{$_("userProfile.yourData.pass1")}" name="firstname" class="shadow form-control">
                    </div>
                    <div class="my-3 col-4">
                        <input type="password" disabled={changePasswordProcessing} bind:value={passCtx.pass2} placeholder="{$_("userProfile.yourData.pass2")}" name="lastname" class="shadow form-control">
                    </div>
                    <div class="my-3 col-4 justify-content-end d-flex">
                        <WaitingButton callback={changePassword} processingCallback={changePasswordProcessingHandle}> 
                            {$_('userProfile.yourData.changePassword')}
                        </WaitingButton>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="my-3 col-8">
                </div>
                <div class="my-3 col-4 justify-content-end text-end pe-4">
                    
                </div>
            </div>
        </div>
    </div>
    <div role="alert" aria-live="assertive" aria-atomic="true" class="toast" data-autohide="false">
        <div class="toast-header">
     
        </div>
        <div class="toast-body">
      
        </div>
      </div>
</LayoutSimple>
