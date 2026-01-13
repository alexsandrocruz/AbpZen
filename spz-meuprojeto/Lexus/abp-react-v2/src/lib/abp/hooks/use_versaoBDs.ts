import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface Get_versaoBDsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function use_versaoBDs(input: Get_versaoBDsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["_versaoBDs", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/-versao-bd", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAll_versaoBDs() {
  return useQuery({
    queryKey: ["_versaoBDs", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/-versao-bd", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function use_versaoBD(id: string) {
  return useQuery({
    queryKey: ["_versaoBD", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/-versao-bd/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreate_versaoBD() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/-versao-bd", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["_versaoBDs"] });
    },
  });
}

export function useUpdate_versaoBD() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/-versao-bd/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["_versaoBDs"] });
      queryClient.invalidateQueries({ queryKey: ["_versaoBD", data.id] });
    },
  });
}

export function useDelete_versaoBD() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/-versao-bd/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["_versaoBDs"] });
    },
  });
}


