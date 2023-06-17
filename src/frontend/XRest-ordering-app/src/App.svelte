<script>
	import Search from './routes/Search.svelte'
	import Retaurant from './routes/restaurant/Retaurant.svelte'
	import Summary from './routes/restaurant/Summary.svelte'
	import Finish from './routes/restaurant/Finish.svelte'
	import Payment from './routes/restaurant/Payment.svelte'
	import PaymentError from './routes/restaurant/PaymentError.svelte'
	import ActivateAccount from './routes/ActivateAccount.svelte'
	import NotFound from './routes/NotFound.svelte'
	import ResetPassword from './routes/ResetPassword.svelte'
	import Logon from './routes/Logon.svelte'
	import Register from './routes/Register.svelte'
	import SendResetPassword from './routes/SendResetPassword.svelte'
	import ProfileFavAddr from './routes/profile/FavAddr.svelte'
	import ProfileOrderHistory  from './routes/profile/OrderHistory.svelte'
	import ProfilePoints from './routes/profile/Points.svelte'
	import ProfileData from './routes/profile/Profile.svelte'
	import Terms from './routes/Terms.svelte'
	import Policy from './routes/Policy.svelte'
	import Cookie from './lib/components/Cookie.svelte'
	import Error from './routes/Error.svelte'
	import { SvelteToast } from '@zerodevx/svelte-toast'
	import { Router } from './lib/router'

	const routes =[
		{ 
			path: '/',
			component: Search,
		},
		{ 
			path: '/index.html',
			component: Search,
		},
		{ 
			path: '/restaurant/(?<alias>.*)/(?<zoneId>.*)/(?<deliveryMode>.*)',
			component: Retaurant,
		},
		{ 
			path: '/summary',
			component: Summary,
		},
		{ 
			path: '/policy' ,
			component: Policy,
		},
		{ 
			path: '/restaurant/(?<restaurantId>.*)/terms',
			component: Terms,
		},
		{ 
			path: '/finish/(?<orderId>.*)',
			component: Finish,
		},
		{ 
			path: '/payment/(?<orderId>.*)',
			component: Payment,
		},
		{ 
			path: '/payment-error',
			component: PaymentError,
		},
		{ 
			path: '/customer/activation/(?<encodedToken>.*)',
			component: ActivateAccount,
		},
		{ 
			path: '/customer/reset-password/(?<encodedToken>.*)',
			component: ResetPassword,
		},
		{ 
			path: '/customer/logon',
			component: Logon,
		},
		{ 
			path: '/customer/register',
			component: Register,
		},
		{ 
			path: '/customer/send-reset-password',
			component: SendResetPassword,
		},
		{ 
			path: '/customer/profile/favorite-addresses',
			component: ProfileFavAddr,
		},
		{ 
			path: '/customer/profile/order-history',
			component: ProfileOrderHistory,
		},
		{ 
			path:'/customer/profile/points',
			component: ProfilePoints,
		},
		{ 
			path: '/customer/profile/data',
			component: ProfileData,
		},
		{ 
			path: '/error' ,
			component: Error,
		},
		{
			path: '.*',
			component: NotFound,
		},
	];

</script>

<Router {routes}/>
<SvelteToast />
<Cookie />
