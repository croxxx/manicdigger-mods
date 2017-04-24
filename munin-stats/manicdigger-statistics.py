#!/usr/bin/python
"""
CollectD script to monitor Manic Digger Server statistics
Plugin Version 1.0 - 2015-11-27
Author: croxxx

"""

import os, sys, urllib2, time

COLLECTD_INTERVAL = os.environ.get('COLLECTD_INTERVAL', '60')
COLLECTD_HOSTNAME = os.environ.get('COLLECTD_HOSTNAME', 'localhost')

#STATUS_URL = 'http://127.0.0.1:25566/munin.stats'
MD_INSTANCE_NAME = 'default'

def print_status(param):
	# given url is used as the base for multiple requests
	suffixes = ['players', 'blocks', 'chat']
	for suffix in suffixes:
		# read each composed url
		statuslist = urllib2.urlopen(param + '.' + suffix).readlines()
		for entry in statuslist:
			# skip comments and empty lines
			if len(entry) <= 1 or entry[0] == '#':
				continue
			key, value = entry.strip('\n').split(': ')
			# output according to format described here: https://collectd.org/wiki/index.php/Plain_text_protocol#PUTVAL
			if suffix == 'players':
				if key == 'players_online':
					print("PUTVAL \"{0}/manicdigger-{3}/players-online\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))
				elif key == 'players_npc':
					print("PUTVAL \"{0}/manicdigger-{3}/players-npc\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))
				elif key == 'players_spectator':
					print("PUTVAL \"{0}/manicdigger-{3}/players-spectator\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))
				elif key == 'players_max':
					print("PUTVAL \"{0}/manicdigger-{3}/players-max\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))
			elif suffix == 'blocks':
				if key == 'blocks_placed':
					print("PUTVAL \"{0}/manicdigger-{3}/blocks-placed\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))
				elif key == 'blocks_destroyed':
					print("PUTVAL \"{0}/manicdigger-{3}/blocks-destroyed\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))
				elif key == 'blocks_used':
					print("PUTVAL \"{0}/manicdigger-{3}/blocks-used\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))
			elif suffix == 'chat':
				if key == 'sent_messages':
					print("PUTVAL \"{0}/manicdigger-{3}/messages-sent\" interval={1} N:{2}".format(COLLECTD_HOSTNAME, COLLECTD_INTERVAL, value, MD_INSTANCE_NAME))

if __name__ == '__main__':
	# We're looking for *exactly* 2 arguments. Script called is also given, so 3 in total.
	if len(sys.argv) < 3:
		print('manicdigger-statistics.py <server status url> <server instance name>')
		sys.exit(1)

	statusurl = sys.argv[1]
	MD_INSTANCE_NAME = sys.argv[2]

	while 1==1:
		print_status(statusurl)
		time.sleep(float(COLLECTD_INTERVAL))
