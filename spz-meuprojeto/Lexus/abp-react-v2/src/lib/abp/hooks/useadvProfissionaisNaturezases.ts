import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProfissionaisNaturezasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProfissionaisNaturezases(input: GetAdvProfissionaisNaturezasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProfissionaisNaturezases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-profissionais-naturezas", {
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

export function useAllAdvProfissionaisNaturezases() {
  return useQuery({
    queryKey: ["advProfissionaisNaturezases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-profissionais-naturezas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProfissionaisNaturezas(id: string) {
  return useQuery({
    queryKey: ["advProfissionaisNaturezas", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-profissionais-naturezas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProfissionaisNaturezas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-profissionais-naturezas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisNaturezases"] });
    },
  });
}

export function useUpdateAdvProfissionaisNaturezas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-profissionais-naturezas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisNaturezases"] });
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisNaturezas", data.id] });
    },
  });
}

export function useDeleteAdvProfissionaisNaturezas() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-profissionais-naturezas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProfissionaisNaturezases"] });
    },
  });
}


