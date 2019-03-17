using System;
using System.Collections.Generic;
using System.IO;
using Console = System.Console;
using File = System.IO.File;
using java.util;
using java.io;
using edu.stanford.nlp.coref;
using edu.stanford.nlp.io;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using edu.stanford.nlp.coref.data;

namespace StanfordNLP.Test
{
    class Program
    {
        static string LoadPrimitiveCorpus(string _path)
        {
            var primitiveCorpus = "";
            primitiveCorpus = File.ReadAllText(_path);

            return primitiveCorpus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static java.lang.Class GetAnnotationClass<T>()
            => ikvm.@internal.ClassLiteral<T>.Value; // = new T().getClass()

        static void Main(string[] args)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.9.2-models.jar`
            var jarRoot = @"..\..\Models\stanford-chinese-corenlp-2018-10-05-models";

            // Text for processing
            var path = @"../../Corpus/AmericanTraderOilSpillAccidentPart.txt";
            var text = LoadPrimitiveCorpus(path);

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
            //props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, coref, sentiment, relation");
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, coref");
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
            //   CC  并列连词           Coordinating conjunction
            //   CD  基数               Cardinal number
            //   DT  限定词             Determiner
            //   EX  存在词             Existential there
            //   FW  外来词             Foreign word
            //   IN  介词               Preposition or subordinating conjunction
            //   JJ  形容词             Adjective
            //  JJR  形容词比较级        Adjective, comparative
            //  JJS  形容词最高级        Adjective, superlative
            //   LS  括号内的数量词       List item marker
            //   MD  情态动词            Modal(can,may,could,might)
            //   NN  名词               Noun, singular or mass
            //  NNS  名词复数            Noun, plural
            //  NNP  专有名词单数        Proper noun, singular
            // NNPS  专有名词复数        Proper noun, plural
            //   NP  专有名词               
            //   NT  词               
            //  PDT  前限定词            Predeterminer
            //  POS  所有格结束词        Possessive ending
            //  PRP  人称代词            Personal pronoun
            // PRP$  物主代词            Possessive pronoun
            //   RB  副词               Adverb
            //  RBR  副词比较级          Adverb, comparative
            //  RBS  副词最高级          Adverb, superlative
            //   RP  助词               Particle
            //  SYM  符号               Symbol
            //   TO                     to
            //   UH  感叹词              Interjection
            //   VB  动词原形            Verb, base form
            //  VBD  动词过去式           Verb, past tense
            //  VBG  动词现在分词         Verb, gerund or present participle
            //  VBN  动词过去分词         Verb, past participle
            //  VBP  动词非第三人称       Verb, non­3rd person singular present
            //  VBZ  动词第三人称单数     Verb, 3rd person singular present
            //  WDT  Wh限定词            Wh­-determiner
            //  WP   Wh代词              Wh­pronoun
            //  WP$  Wh物主代词          Possessive wh-­pronoun
            //  WRB  Wh副词              Wh -­adverb
            //props.setProperty("pos.model", "edu/stanford/nlp/models/pos-tagger/chinese-distsim/chinese-distsim.tagger");


            // Named Entity Recognition – NERClassifierCombiner
            // The Stanford Named Entity Recognizer identifies tokens that are proper nouns as members of specific classes such as Person(al) name, Organization name etc.
            // 需要说明的一点是NERClassifierCombiner类依赖lemma（词元）解释器，但是参考（https://stanfordnlp.github.io/CoreNLP/human-languages.html）说明中文是不支持Lemma解释器的，
            // 依据实例，似乎Lemma类直接将对应的Text拿来作为结果
            //props.setProperty("ner.language", "chinese");
            //props.setProperty("ner.model", "edu/stanford/nlp/models/ner/chinese.misc.distsim.crf.ser.gz");
            //props.setProperty("ner.applyNumericClassifiers", "true");
            // Whether or not to use SUTime. SUTime at present only supports English; if not processing English, make sure to set this to false.
            // Numeric Entity Label
            // DATE
            // TIME
            // DURATION
            // SET
            // MONEY
            // PERCENT
            // NUMBER
            // Normalized Named Entity Label
            // PERSON
            // LOCATION
            // ORGANIZATION
            // MISC 
            // 可以训练自己的CRF(Conditional Random Field) sequence model（https://nlp.stanford.edu/software/crf-faq.html#a）
            //props.setProperty("ner.useSUTime", "false");
            //props.setProperty("sutime.markTimeRanges", "true");

            // Stanford RegexNER
            // 需要在项目中补充适用于情景推演的RegexNER规则文件（https://nlp.stanford.edu/software/regexner.html）
            // run fine-grained NER with a custom rules file
            props.setProperty("ner.fine.regexner.mapping", "edu/stanford/nlp/models/kbp/chinese/cn_regexner_mapping.tab;edu/stanford/nlp/models/kbp/chinese/cn_scenario_library_custom_regexner_mapping.tab");
            //props.setProperty("ner.additional.regexner.mapping", "");
            //props.setProperty("ner.fine.regexner.noDefaultOverwriteLabels", "CITY,COUNTRY,STATE_OR_PROVINCE");

            // SentimentAnnotator
            // 似乎某种程度上也支持中文情感解释器

            // ParserAnnotator
            // The Stanford Parser analyses and annotates the syntactic structure of each sentence in the text. The Stanford Parer is actually not just one parser, but offers phrase structure parses as well as dependency parses.
            // 在使用ParserAnnotator的时候需要注意指定Parse依赖的Dependencies
            // Since version 3.5.2 the Stanford Parser and Stanford CoreNLP output grammatical relations in the Universal Dependencies v1 representation by default. (https://nlp.stanford.edu/software/stanford-dependencies.html)
            // Standard Stanford dependencies (collapsed and propagated)
            // Universal Dependencies

            // Neural Network Dependency Parser
            // A dependency parser analyzes the grammatical structure of a sentence, establishing relationships between "head" words and words which modify those heads.(https://nlp.stanford.edu/software/nndep.html)
            //props.setProperty("parse.model", "edu/stanford/nlp/models/srparser/chineseSR.ser.gz");
            props.setProperty("parse.originalDependencies", "true");

            // DependencyParseAnnotator
            // Provides a fast syntactic dependency parser
            // Three dependency-based outputs
            // Basic, uncollapsed dependencies saved in BasicDependenciesAnnotation
            // Collapsed dependencies, saved in CollapsedDependenciesAnnotation
            // Collapsed dependencies with processed coordinations, in CollapsedCCProcessedDependenciesAnnotation
            //props.setProperty("depparse.model", "edu/stanford/nlp/models/parser/nndep/UD_Chinese.gz");
            //props.setProperty("depparse.language", "chinese");

            // CorefAnnotator
            // The Stanford CorefAnnotator implements pronominal and nominal coreference resolution.
            // 
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

            // RelationExtractorAnnotator
            // Stanford relation extractor is a Java implementation to find relations between two entities
            // Included with Stanford relation extractor is a classifier to predict relations 
            // Live_In
            // Located_In
            // OrgBased_In
            // Work_For
            // None

            //props.setProperty("kbp.semgrex", "edu/stanford/nlp/models/kbp/chinese/semgrex");
            //props.setProperty("kbp.tokensregex", "edu/stanford/nlp/models/kbp/chinese/tokensregex");
            //props.setProperty("kbp.language", "zh");
            //props.setProperty("kbp.model", "none");

            //props.setProperty("entitylink.wikidict", "edu/stanford/nlp/models/kbp/chinese/wikidict_chinese.tsv.gz");            
            var pipeline = new StanfordCoreNLP(props);

            // CoreNLP’s core package includes two classes: Annotation and Annotator.
            // Annotation
            // are data structures that hold the results of the annotators.Annotations are generally maps.
            // Annotators
            // are more like functions, but they operate on Annotations rather than Objects.
            var document = new Annotation(text);
            pipeline.annotate(document);

            // Result - Pretty Print
            //using (var stream = new ByteArrayOutputStream())
            //{
            //    pipeline.prettyPrint(document, new PrintWriter(stream));
            //    Console.WriteLine(stream.toString());
            //    stream.close();
            //}

            var corefChainAnnotation = new CorefCoreAnnotations.CorefChainAnnotation().getClass();
            var corefMentionsAnnotation = new CorefCoreAnnotations.CorefMentionsAnnotation().getClass();
            var namedEntityTagAnnotation = new CoreAnnotations.NamedEntityTagAnnotation().getClass();
            var normalizedNamedEntityTagAnnotation = GetAnnotationClass<CoreAnnotations.NormalizedNamedEntityTagAnnotation>();
            var partOfSpeechAnnotation = new CoreAnnotations.PartOfSpeechAnnotation().getClass();
            var sentencesAnnotation = new CoreAnnotations.SentencesAnnotation().getClass();
            var textAnnotation = new CoreAnnotations.TextAnnotation().getClass();
            var tokensAnnotation = new CoreAnnotations.TokensAnnotation().getClass();


            // these are all the sentences in this document
            var sentences = document.get(sentencesAnnotation) as ArrayList;
            var documentSplitByTimeStamp = new List<string>();
            var words = new List<string>();
            var posTags = new List<string>();
            var nerTags = new List<string>();
            var nnerTags = new List<string>();

            var hasDateMention = false;
            var partDocument = "";

            foreach (CoreMap sentence in sentences.toArray())
            {
                var sentenceText = sentence.get(textAnnotation) as string;

                // traversing the words in the current sentence
                var corefMentions = sentence.get(corefMentionsAnnotation) as ArrayList;
                foreach (Mention m in corefMentions)
                {
                    if (m.nerString == "DATE")
                    {
                        hasDateMention = true;
                        break;
                    }
                    //Console.WriteLine("\t" + m);
                }

                if (hasDateMention)
                {
                    documentSplitByTimeStamp.Add(partDocument);
                    hasDateMention = false;
                    partDocument = sentenceText;
                }
                else
                {
                    partDocument += sentenceText;
                }

                var tokens = sentence.get(tokensAnnotation) as ArrayList;
                foreach (CoreLabel token in tokens.toArray())
                {
                    // this is the text of the token
                    var word = token.get(textAnnotation) as string;
                    words.Add(word);

                    // this is the POS tag of the token
                    var pos = token.get(partOfSpeechAnnotation) as string;
                    posTags.Add(pos);

                    // this is the this is the NER label of the token
                    var ner = token.get(namedEntityTagAnnotation) as string;
                    nerTags.Add(ner);

                    var nner = token.get(normalizedNamedEntityTagAnnotation) as string;
                    nnerTags.Add(nner);
                }
            }

            documentSplitByTimeStamp.Add(partDocument);

            Console.WriteLine(documentSplitByTimeStamp);


            Directory.SetCurrentDirectory(curDir);
        }
    }
}
