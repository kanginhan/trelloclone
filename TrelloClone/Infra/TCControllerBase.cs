using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrelloClone.Infra
{
    /// <summary>
    /// TCControllerBase
    /// : Action에는 로직을 두지 않는다.
    /// 1) 간단한 경우 Repository만 사용한다
    /// 2) 간단하지 않으면 Service를 사용한다
    /// </summary>
    public class TCControllerBase : ControllerBase
    {
        protected IMapper _mapper;
        protected ILogger<TCControllerBase> _logger;
        protected readonly Config _config;

        public TCControllerBase(IMapper mapper, ILogger<TCControllerBase> logger, IOptions<Config> config)
        {
            _mapper = mapper;
            _logger = logger;
            _config = config.Value;
        }
    }
}
