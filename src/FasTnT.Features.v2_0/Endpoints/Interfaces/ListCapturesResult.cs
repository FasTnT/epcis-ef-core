﻿using FasTnT.Domain.Model;

namespace FasTnT.Features.v2_0.Endpoints.Interfaces;

public record ListCapturesResult(IEnumerable<Request> Captures);
