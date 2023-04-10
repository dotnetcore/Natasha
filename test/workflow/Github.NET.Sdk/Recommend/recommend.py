# -*- coding: utf-8 -*-
import jieba
import numpy as np
import re
import os
import json
import sys
 

class Result:
    def __init__(self, cid, number, title, url, dist, output):
        self.id = cid
        self.number = number
        self.title = title
        self.url = url
        self.dist = dist
        self.output = output

    def __str__(self):
        return json.dumps(dict(self), ensure_ascii=False)

def get_word_list(src_str,paddle_shut):
    cut = sorted(clean_stopword(jieba.cut(src_str,use_paddle=paddle_shut)))
    return (','.join(cut)).split(',')

def get_word_vector(list_word1,list_word2):
    """
    :param s1: 句子1
    :param s2: 句子2
    :return: 返回句子的余弦相似度
    """
    # 列出所有的词,取并集
    key_word = list(set(list_word1 + list_word2))
    # 给定形状和类型的用0填充的矩阵存储向量
    word_vector1 = np.zeros(len(key_word))
    word_vector2 = np.zeros(len(key_word))
 
    # 计算词频
    # 依次确定向量的每个位置的值
    for i in range(len(key_word)):
        # 遍历key_word中每个词在句子中的出现次数
        for j in range(len(list_word1)):
            if key_word[i] == list_word1[j]:
                word_vector1[i] += 1
        for k in range(len(list_word2)):
            if key_word[i] == list_word2[k]:
                word_vector2[i] += 1
    # 输出向量
    #print(word_vector1)
    #print(word_vector2)
    return word_vector1, word_vector2
 
 
def clean_stopword(word_cut):
    cleanword = []
    for word in word_cut:
        word = word.lower()
        if word not in stopword_list:
            cleanword.append(word)
    return cleanword
 
 
def cos_dist(vec1,vec2):
    """
    :param vec1: 向量1
    :param vec2: 向量2
    :return: 返回两个向量的余弦相似度
    """
    dist1=float(np.dot(vec1,vec2)/(np.linalg.norm(vec1)*np.linalg.norm(vec2)))
    return dist1
 
def filter_html(html):
    """
    :param html: html
    :return: 返回去掉html的纯净文本
    """
    dr = re.compile(r'<[^>]+>',re.S)
    dd = dr.sub('',html).strip()
    return dd


def pick_result(results):
    """
    挑选出合适的结果
    """
    compareIndex = 0
    pick = []
    for compare_item in results:
        if compareIndex == len(compareMaxValueArray):
            break
        if len(pick) < compareCountArray[compareIndex]:
            if compareMaxValueArray[compareIndex] < compare_item.dist:
                continue
            elif compareMinValueArray[compareIndex] < compare_item.dist and compare_item.dist <= compareMaxValueArray[compareIndex]:
                pick.append(compare_item.__dict__)
                if len(pick) >= compareCountArray[compareIndex]:
                    compareIndex = compareIndex + 1
            else:
                for i in range(compareCountArray[compareIndex] - len(pick)):
                    pick.append(None)
                while True:
                    compareIndex = compareIndex + 1
                    if compareIndex == len(compareMaxValueArray):
                        break
                    if compareMinValueArray[compareIndex] < compare_item.dist and compare_item.dist <= compareMaxValueArray[compareIndex]:
                        pick.append(compare_item.__dict__)
                        if len(pick) >= compareCountArray[compareIndex]:
                            compareIndex = compareIndex + 1
                        break
            
    return pick 

stopword_list=[]
compareCountArray=[]
compareMinValueArray=[]
compareMaxValueArray=[]

if __name__ == '__main__':

    log="------start------"
    try:
        json_path = os.path.join(os.getcwd(),"compare.json")
        stopwords_path = os.path.join(os.getcwd(),"stopwords.txt");
        result_path = os.path.join(os.getcwd(),"recommend.json")

        if not os.path.exists(json_path):
            log = log + " 未找到对比文件:" + json_path
            quit(1)
        else:
            log = log + " 已找到对比文件！"

        if not os.path.exists(stopwords_path):
            log = log + " 未找到停用词文件:" + stopwords_path
            quit(1)
        else:
            log = log + " 已找到停用词文件！"

        with open(json_path,"r") as json_file:
            obj = json.loads(json_file.read())
        log = log + " 已读取对比文件！"

        with open(stopwords_path,"r") as file:
            stopword_list = file.read().splitlines()
        log = log + " 已读取停用词文件！"

        source = obj.get("Source")
        compareCountArray = obj.get("PickCount")
        compareMinValueArray = obj.get("PickMinValues")
        compareMaxValueArray = obj.get("PickMaxValues")
        log = log +" 源语句：" + source
        compares = obj.get("Reference")

        results=[]
        use_paddle = False
        use_paddle_env=os.environ.get("USE_PADDLE");
        if use_paddle_env == "TRUE":
            use_paddle = True
            log = log + " 启用 Paddle 模式！"

        src_list = get_word_list(source,use_paddle)
        log = log +"源分词:\t"+'/'.join(src_list)
        sys.stderr.write(log)

        for compare_item in compares:
            compare_str = compare_item.get("Title")
            compare_list = get_word_list(compare_str, use_paddle)
            vec1,vec2 = get_word_vector(src_list,compare_list)
            dist = cos_dist(vec1,vec2)
            results.append(Result(compare_item.get("Id"), compare_item.get("Number"), compare_str,compare_item.get("Url"), dist, "{:.2%}".format(dist)))
        log = log + " 已完成相似度计算"
        
        results.sort(key=lambda x: x.dist, reverse=True)
        pick = pick_result(results)
        with open(result_path, 'w') as file_handler:
             file_handler.write(json.dumps(pick))
        log = log + " 已将结果写入文件"
        quit(0)

    except Exception as e:
        sys.stderr.write(str(e) + log)
        quit(1)
    


