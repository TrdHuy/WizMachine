#pragma once
#ifndef _CALLBACK_INTERNAL_H_
#define _CALLBACK_INTERNAL_H_

#include <functional>

using ProgressCallbackInternal = std::function<void(int, const char*)>;

#endif //#ifndef _CALLBACK_INTERNAL_H_
