import requests
import json
from lxml import etree

url = "https://s.weibo.com/top/summary?Refer=top_hot&topnav=1"

webpage = requests.get(url)

html = etree.HTML(webpage.text)

news = html.xpath('//td[@class="td-02"]/a/text()')
views = html.xpath('//td[@class="td-02"]/span/text()')
links = html.xpath('//td[@class="td-02"]/a/@href')

news.pop(0)
links.pop(0)

result = json.dumps([news, views, links])

with open('./WeiboHot.json', 'w', encoding = 'utf-8') as f:
    f.write(result)



