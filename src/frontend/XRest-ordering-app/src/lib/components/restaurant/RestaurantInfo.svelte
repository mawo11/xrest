<script>
  import { selectedRestaurant } from "../../stores"
  import { _ } from "../../services/localization/i18n"
</script>

<div class="card w-100 mx-auto d-none d-sm-none d-md-block">
  <div class="card-header bg-dark text-light text-end">
      <a data-bs-toggle="collapse" role="button" aria-controls="body-restaurant" href="#body-restaurant" class="text-decoration-none link-light stretched-link"><i class="fas fa-window-minimize"></i></a>
  </div>
  <div class="card-body collapse show" id="body-restaurant">
    <h5 class="card-title text-center">{$_('restaurantInfo.restaurant')}</h5>
    <hr>
    {$selectedRestaurant.name}<br/>
    
    {$_("menu.addressLabel")} : {$selectedRestaurant.city}, {$selectedRestaurant.address}
    {#if $selectedRestaurant.working}
      <p><i class="fas fa-door-open text-success"></i> {$_('restaurantInfo.delivery')} {$selectedRestaurant.onlineFrom} - {$selectedRestaurant.onlineTo}. <span class="fw-bold text-success"> {$_("menu.welcome")}</span></p>
    {:else}
    <p><i class="fas fa-door-closed text-danger"></i> {$_('restaurantInfo.delivery')} {$selectedRestaurant.onlineFrom} - {$selectedRestaurant.onlineTo}. <span class="fw-bold text-danger"> {$_("menu.closed")}</span></p>
    {/if}
  </div>
</div>
