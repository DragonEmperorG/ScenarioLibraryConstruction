using System;
using System.IO;
using java.util;
using java.io;
using edu.stanford.nlp.io;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.time;
using Console = System.Console;

namespace StanfordNLP.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.9.2-models.jar`
            var jarRoot = @"..\..\Models\stanford-chinese-corenlp-2018-10-05-models";

            // Text for processing
            //var text = "Kosgi Santosh sent an email to Stanford University. He didn't get a reply.";
            var text = "2018年9月10日，山竹继续向西偏南移动，在关岛附近海域掠过。多个气象机构的预测分歧都逐渐收窄，均预测山竹会经过巴士海峡或掠过台湾南部，移入南海北部，对该区构成威胁；山竹在同日上午8时进入香港天文台责任范围，香港天文台评定其为台风；同时，山竹继续受到干空气入侵，并移到风切变较为强的海域，令它发展迟缓，迟迟未能开启风眼，但国家气象中心和香港天文台在晚上8时仍然将其升格为强台风。";

            // Annotation pipeline configuration
            // set up pipeline properties
            var props = new Properties();
            // We should change current directory, so StanfordCoreNLP could find all the model files automatically
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            // build pipeline
            props.load(IOUtils.readerFromString("StanfordCoreNLP-chinese.properties"));
            // set the list of annotators to run
            // 参考官方提供的StanfordCoreNLP-chinese.properties属性文件
            // Stanford CoreNLP对中文支持的解释器（Annotator）有：
            // Tokenize / Segment       ✔
            // Sentence Split           ✔
            // Part of Speech           ✔
            // Lemma
            // Named Entities           ✔
            // Constituency Parsing     ✔
            // Dependency Parsing       ✔
            // Sentiment Analysis
            // Mention Detection        ✔
            // Coreference              ✔
            //props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, coref");
            // set a property for an annotator,
            // TokenizerAnnotator
            // The tokenizer subdivides a text into individual tokens, i.e. words, punctuation marks etc.
            // TokenizerAnnotator 负责将一个句子分解为序列化的独立的token值
            // [Text=2018年 CharacterOffsetBegin=0 CharacterOffsetEnd=5 PartOfSpeech=NT Lemma=2018年 NamedEntityTag=DATE NormalizedNamedEntityTag=2018-09-10]
            //props.setProperty("tokenize.language", "zh");

            //props.setProperty("segment.model", "edu/stanford/nlp/models/segmenter/chinese/ctb.gz");
            //props.setProperty("segment.sighanCorporaDict", "edu/stanford/nlp/models/segmenter/chinese");
            //props.setProperty("segment.serDictionary", "edu/stanford/nlp/models/segmenter/chinese/dict-chris6.ser.gz");
            //props.setProperty("segment.sighanPostProcessing", "true");

            // WordsToSentenceAnnotator
            // The sentence splitter segments a text into sentences
            // WordsToSentenceAnnotator 将序列化后的token值划分成句子单元
            // 中文通常采用如下的正则表达式来划分句子单元
            //props.setProperty("ssplit.boundaryTokenRegex", "[.。]|[!?！？]+");

            // POSTaggerAnnotator
            // The Stanford Part of Speech Tagger, assigns word class labels to each token according to a model and annotation scheme
            // 
            //props.setProperty("pos.model", "edu/stanford/nlp/models/pos-tagger/chinese-distsim/chinese-distsim.tagger");


            // Named Entity Recognition – NERClassifierCombiner
            // The Stanford Named Entity Recognizer identifies tokens that are proper nouns as members of specific classes such as Person(al) name, Organization name etc.
            // 需要说明的一点是NERClassifierCombiner类依赖lemma（词元）解释器，但是参考（https://stanfordnlp.github.io/CoreNLP/human-languages.html）说明中文是不支持Lemma解释器的，
            // 依据实例，似乎Lemma类直接将对应的Text拿来作为结果
            //props.setProperty("ner.language", "chinese");
            //props.setProperty("ner.model", "edu/stanford/nlp/models/ner/chinese.misc.distsim.crf.ser.gz");
            //props.setProperty("ner.applyNumericClassifiers", "true");
            //props.setProperty("ner.useSUTime", "false");

            // Stanford RegexNER
            // 需要在项目中补充适用于情景推演的RegexNER规则文件（https://nlp.stanford.edu/software/regexner.html）
            // run fine-grained NER with a custom rules file
            props.setProperty("ner.fine.regexner.mapping", "edu/stanford/nlp/models/kbp/chinese/cn_regexner_mapping.tab;edu/stanford/nlp/models/kbp/chinese/cn_scenario_library_custom_regexner_mapping.tab");
            //props.setProperty("ner.additional.regexner.mapping", "");
            //props.setProperty("ner.fine.regexner.noDefaultOverwriteLabels", "CITY,COUNTRY,STATE_OR_PROVINCE");

            //props.setProperty("parse.model", "edu/stanford/nlp/models/srparser/chineseSR.ser.gz");

            //props.setProperty("depparse.model", "edu/stanford/nlp/models/parser/nndep/UD_Chinese.gz");
            //props.setProperty("depparse.language", "chinese");

            //props.setProperty("coref.sieves", "ChineseHeadMatch, ExactStringMatch, PreciseConstructs, StrictHeadMatch1, StrictHeadMatch2, StrictHeadMatch3, StrictHeadMatch4, PronounMatch");
            //props.setProperty("coref.input.type", "raw");
            //props.setProperty("coref.postprocessing", "true");
            //props.setProperty("coref.calculateFeatureImportance", "false");
            //props.setProperty("coref.useConstituencyTree", "true");
            //props.setProperty("coref.useSemantics", "false");
            //props.setProperty("coref.algorithm", "hybrid");
            //props.setProperty("coref.path.word2vec", "");
            //props.setProperty("coref.language", "zh");
            //props.setProperty("coref.defaultPronounAgreement", "true");
            //props.setProperty("coref.zh.dict", "edu/stanford/nlp/models/dcoref/zh-attributes.txt.gz");
            //props.setProperty("coref.print.md.log", "false");
            //props.setProperty("coref.md.type", "RULE");
            //props.setProperty("coref.md.liberalChineseMD", "false");

            //props.setProperty("kbp.semgrex", "edu/stanford/nlp/models/kbp/chinese/semgrex");
            //props.setProperty("kbp.tokensregex", "edu/stanford/nlp/models/kbp/chinese/tokensregex");
            //props.setProperty("kbp.language", "zh");
            //props.setProperty("kbp.model", "none");

            //props.setProperty("entitylink.wikidict", "edu/stanford/nlp/models/kbp/chinese/wikidict_chinese.tsv.gz");            
            var pipeline = new StanfordCoreNLP(props);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }

            Directory.SetCurrentDirectory(curDir);
        }
    }
}
