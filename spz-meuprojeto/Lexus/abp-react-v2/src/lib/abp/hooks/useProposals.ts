import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProposalsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  clientId?: string;
  }

export function useProposals(input: GetProposalsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["proposals", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal", {
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

export function useAllProposals() {
  return useQuery({
    queryKey: ["proposals", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useProposal(id: string) {
  return useQuery({
    queryKey: ["proposal", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/proposal/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProposal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/proposal", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposals"] });
    },
  });
}

export function useUpdateProposal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/proposal/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["proposals"] });
      queryClient.invalidateQueries({ queryKey: ["proposal", data.id] });
    },
  });
}

export function useDeleteProposal() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/proposal/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposals"] });
    },
  });
}


